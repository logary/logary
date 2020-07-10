﻿/// A module defining the types relevant for targets to implement
/// and methods to interact with those targets.
namespace Logary

open Hopac
open Hopac.Infixes
open Logary.Internals

/// A target configuration is the 'reference' to the to-be-run target while it
/// is being configured, and before Logary fully starts up.
type TargetConf =
  { name: string
    rules: Rule list
    bufferSize: uint16
    /// Supervision policy. Use `Logary.Internals.Policy` to choose. By default,
    /// uses exponential backup with a maximum delay, retrying forever.
    policy: Policy
    /// (goes on specific target) (composes when creating target, not compose at call-site when sending message)
    middleware: Middleware list
    server: TargetAPI -> Job<unit> }

  override x.ToString() =
    sprintf "TargetConf(%s)" x.name

  static member ofLogger (logger: Logger) =
    { name=logger.name.ToString()
      rules=[ Rule.empty ]
      bufferSize=1us
      policy=Policy.terminate
      middleware=[]
      server=Target.ofLogger logger
    }

module TargetConf =
  let create policy bufferSize server name: TargetConf =
    let will = Will.create ()
    { name = name
      rules = Rule.empty :: []
      bufferSize = bufferSize
      policy = policy
      middleware = []
      server = server will }

  let createSimple server name: TargetConf =
    { name = name
      rules = Rule.empty :: []
      bufferSize = 512us
      policy = Policy.exponentialBackoffForever
      middleware = []
      server = server }

  let bufferSize size conf =
    { conf with bufferSize = size }

  let setRule r conf =
    { conf with rules = r :: [] }

  /// specific rule should comes last, be careful with the add order
  let addRule r conf =
    { conf with rules = r :: conf.rules }

  let policy policy conf =
    { conf with policy = policy }

  let middleware mid conf =
    { conf with middleware = mid :: conf.middleware }

﻿module Libryy.Core

// Note: this library has no reference to Logary proper
open Libryy.Logging

let coreLogger = Log.create "Libryy.Core"

let work (logger : Logger) =
  fun () ->
      Message.event Warn "Hey {user}!"
      |> Message.setFieldValue "user" "haf"
      |> Message.setSingleName "Libryy.Core.work"
      |> Message.setTimestamp 1470047883029045000L
  |> logger.logWithAck Warn
  |> Async.RunSynchronously

  42

let workNonAsync (logger : Logger) =
  fun () ->
      Message.event Warn "Hey {user}!"
      |> Message.setFieldValue "user" "haf"
      |> Message.setSingleName "Libryy.Core.work"
      |> Message.setTimestamp 1470047883029045000L
  |> logger.log Warn
  45

let simpleWork (logger : Logger) =
  logger.logSimple (Message.event Error "Too simplistic")
  43

let staticWork () =
  coreLogger.logSimple (Message.event Debug "A debug log")
  49
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string _commandID;
    private string _commandDesc;
    private string _commandFormat;

    public string commandID { get { return _commandID; } }
    public string commandDesc { get { return _commandDesc; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string desc, string format)
    {
        _commandID = id;
        _commandDesc = desc;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string desc, string format, Action command) : base (id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string id, string desc, string format, Action<T1> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

public class DebugCommand<T1, T2> : DebugCommandBase
{
    private Action<T1, T2> command;

    public DebugCommand(string id, string desc, string format, Action<T1, T2> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value0, T2 value1)
    {
        command.Invoke(value0, value1);
    }
}

public class DebugCommand<T1, T2, T3> : DebugCommandBase
{
    private Action<T1, T2, T3> command;

    public DebugCommand(string id, string desc, string format, Action<T1, T2, T3> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value0, T2 value1, T3 value2)
    {
        command.Invoke(value0, value1, value2);
    }
}

public class DebugCommand<T1, T2, T3, T4> : DebugCommandBase
{
    private Action<T1, T2, T3, T4> command;

    public DebugCommand(string id, string desc, string format, Action<T1, T2, T3, T4> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value0, T2 value1, T3 value2, T4 value3)
    {
        command.Invoke(value0, value1, value2, value3);
    }
}
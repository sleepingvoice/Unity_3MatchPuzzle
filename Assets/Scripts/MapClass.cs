using System;
using System.Collections.Generic;

public class MapClass
{
    public int MapNumber = 0;
    public List<JsonList> BoolList;
}

[Serializable]
public class JsonList
{
    public List<int> JsonValue;
}
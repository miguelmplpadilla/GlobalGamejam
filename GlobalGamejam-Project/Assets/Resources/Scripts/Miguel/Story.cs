using System;
using System.Collections.Generic;

[Serializable]
public class Passage {
    public string text;
    public List<Link> links;
    public string name;
    public string pid;
    public Position position;
}

[Serializable]
public class Link {
    public string name;
    public string link;
    public string pid;
}

[Serializable]
public class Position {
    public string x;
    public string y;
}

[Serializable]
public class Story {
    public List<Passage> passages;
    public string name;
    public string startnode;
    public string creator;
    public string creator_version;
    public string ifid;
}
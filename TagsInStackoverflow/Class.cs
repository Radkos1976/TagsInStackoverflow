
public class Rootobject
{
    public Item[] items { get; set; }
    public bool has_more { get; set; }
    public int quota_max { get; set; }
    public int quota_remaining { get; set; }
}

public class Item
{
    public bool has_synonyms { get; set; }
    public bool is_moderator_only { get; set; }
    public bool is_required { get; set; }
    public int count { get; set; }
    public string name { get; set; }
    public Collective[] collectives { get; set; }
}

public class Collective
{
    public string[] tags { get; set; }
    public External_Links[] external_links { get; set; }
    public string description { get; set; }
    public string link { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class External_Links
{
    public string type { get; set; }
    public string link { get; set; }
}

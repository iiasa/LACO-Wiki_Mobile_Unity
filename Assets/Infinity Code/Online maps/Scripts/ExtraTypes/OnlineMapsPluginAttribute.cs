using System;

public class OnlineMapsPluginAttribute: Attribute
{
    public readonly bool enabledByDefault;
    public readonly string title;
    public readonly Type requiredType;
    public readonly string group;

    public OnlineMapsPluginAttribute(string title, Type requiredType, bool enabledByDefault = false)
    {
        this.enabledByDefault = enabledByDefault;
        this.title = title;
        this.requiredType = requiredType;
    }

    public OnlineMapsPluginAttribute(string title, Type requiredType, string group): this(title, requiredType)
    {
        this.group = group;
    }
}
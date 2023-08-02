using System;

namespace DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class DIAttribute : Attribute
    {
        public string Id { get; }

        public DIAttribute(string id = "")
        {
            Id = id;
        }
    }
}
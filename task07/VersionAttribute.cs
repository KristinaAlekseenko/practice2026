using System;

namespace task07
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public int _major { get; }

        public int _minor { get; }


        public VersionAttribute(int major, int minor)
        {
            _major = major;

            _minor = minor;

        }
    }
}

using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalItemAttribute : PropertyAttribute
    {
        public string propertyName;
        public object[] propertyValue;

        public ConditionalItemAttribute(string propertyName, object propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = new object[] { propertyValue };
        }

        public ConditionalItemAttribute(string propertyName, object[] propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }
    }
}

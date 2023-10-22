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
        public CardTypeEnum propertyValue;

        public ConditionalItemAttribute(string propertyName, CardTypeEnum propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }
    }
}

using System;

public class OnlineMapsWizardControlHelperAttribute : Attribute
{
    public OnlineMapsTarget resultType;

    public OnlineMapsWizardControlHelperAttribute(OnlineMapsTarget resultType)
    {
        this.resultType = resultType;
    }
}
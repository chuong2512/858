
[System.Serializable]
public class BoolReference
{
    public bool UseConstant;
    public bool ConstantValue;
    public BoolVariable Variable;

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

}

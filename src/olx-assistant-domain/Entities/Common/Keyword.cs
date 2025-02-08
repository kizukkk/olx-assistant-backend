namespace olx_assistant_domain.Entities.Common;
public class Keyword : BaseEntity
{
    public string Word { get; set; }
    public float Value { get; set; }

    public Keyword(string word, float value)
    {
        Word = word;
        Value = value;
    }
}

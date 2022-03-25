namespace EleCho.Json
{
    public enum JsonTokenKind
    {
        None,        // no token
        ObjectStart, // {
        ObjectEnd,   // }
        ArrayStart,  // [
        ArrayEnd,    // ]
        Colon,       // :
        Comma,       // ,
        String,      // "..."
        Number,      // 123.456
        True,        // true
        False,       // false
        Null         // null
    }
}
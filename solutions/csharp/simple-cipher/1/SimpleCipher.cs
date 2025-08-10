using System.Text;

public class SimpleCipher
{
    private const int LastCharInAlphabet = 122; //lowercase z
    private const int FirstCharInAlphabet = 97; //lowercase a
    private readonly char[] _key;
    public SimpleCipher()
    {
        var random = new Random();

        StringBuilder sb = new StringBuilder();
        _key = new char[100];
        for (int i = 0; i < 100; i++)
        {
            var lower = (char)random.Next(FirstCharInAlphabet, LastCharInAlphabet);
            sb.Append(lower);
            _key[i] = lower;
        }
    }

    public SimpleCipher(string key){
        _key = key.ToCharArray();
    }

    public string Key => new(_key);

    public string Encode(string plaintext) => Shift(plaintext, CipherMode.Encode);

    public string Decode(string ciphertext) => Shift(ciphertext, CipherMode.Decode);

    private enum CipherMode
    {
        Encode,
        Decode
    }
    
    private string Shift(string inputString, CipherMode mode = CipherMode.Encode)
    {
        var inputChars = inputString.ToCharArray();

        var shiftedChars = inputChars
            .Zip(RepeatingKey(), (inputChar, keyChar) => 
                mode == CipherMode.Encode ?
                    EncodeChar(inputChar, keyChar) :
                    DecodeChar(inputChar, keyChar)
            );

        return new string(shiftedChars.ToArray());
    }

    private static char DecodeChar(char plaintextChar, char keyChar)
    {
        var shift = keyChar - FirstCharInAlphabet;
        var shiftedLeft = plaintextChar - shift;

        // wrap from right
        if (shiftedLeft < FirstCharInAlphabet)
        {
            var diff = plaintextChar - keyChar + 1;
            return (char)(LastCharInAlphabet + diff);
        }
        
        return (char)shiftedLeft;
    }
    
    private static char EncodeChar(char plaintextChar, char keyChar)
    {
        var shift = (keyChar - FirstCharInAlphabet);
        var shiftedRight = plaintextChar + shift;

        // wrap from left
        if (shiftedRight > LastCharInAlphabet)
        {
            return (char)(96 + (shiftedRight - LastCharInAlphabet));
        }

        return (char)(shiftedRight);
    }

    private IEnumerable<char> RepeatingKey()
    {
        while (true)
        {   
            foreach (var c in _key)
            {
                yield return c;
            }
        }
    }
}
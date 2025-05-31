using System.Security.Cryptography;
using System.Text;

static class Hash{


    public static string get(String password){

        return hashMD5(password);
        
    }

    static String  hashMD5(String text)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = MD5.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(text));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }
}
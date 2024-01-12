using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public partial class insuredPerson
{
    public string DecryptCC()
    {
        string input = ccNo;
        if (string.IsNullOrEmpty(input))
            return null;
        byte[] salt = Encoding.UTF8.GetBytes(ApplicationConstants.SALTENCRYPTION);
        byte[] encryptedBytes = Convert.FromBase64String(input);
        AesManaged aes = new AesManaged();
        Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(ApplicationConstants.PASSENCRYPTION, salt);
        aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
        aes.KeySize = aes.LegalKeySizes[0].MaxSize;
        aes.Key = rfc.GetBytes(aes.KeySize / 8);
        aes.IV = rfc.GetBytes(aes.BlockSize / 8);
        ICryptoTransform cypher = aes.CreateDecryptor();
        MemoryStream memStream = new MemoryStream();
        CryptoStream decryptor = new CryptoStream(memStream, cypher, CryptoStreamMode.Write);
        decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
        decryptor.Flush();
        decryptor.Close();
        byte[] decryptedBytes = memStream.ToArray();
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public void EncryptCC(string ccNo)
    {
        byte[] data = Encoding.UTF8.GetBytes(ccNo);
        byte[] salt = Encoding.UTF8.GetBytes(ApplicationConstants.SALTENCRYPTION);
        AesManaged aes = new AesManaged();
        Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(ApplicationConstants.PASSENCRYPTION, salt);
        aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
        aes.KeySize = aes.LegalKeySizes[0].MaxSize;
        aes.Key = rfc.GetBytes(aes.KeySize / 8);
        aes.IV = rfc.GetBytes(aes.BlockSize / 8);
        ICryptoTransform cypher = aes.CreateEncryptor();
        MemoryStream memStream = new MemoryStream();
        CryptoStream encryptor = new CryptoStream(memStream, cypher, CryptoStreamMode.Write);
        encryptor.Write(data, 0, data.Length);
        encryptor.Flush();
        encryptor.Close();
        byte[] encryptedBytes = memStream.ToArray();
        ccNo = Convert.ToBase64String(encryptedBytes);
    }
}

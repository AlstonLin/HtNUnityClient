public class HashGenerator {
  static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  static System.Random random = new System.Random();

  public static string generateHash (){
  char[] stringChars = new char[8];

  for (int i = 0; i < stringChars.Length; i++)
  {
      stringChars[i] = chars[random.Next(chars.Length)];
  }

  return new string(stringChars);
  }
}

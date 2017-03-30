using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFInterpreter
{
    class Program
    {

        //BF+
        // Using the standard BF characters plus a few extra to make it not so tedious.
        // Standard characters:
        // > : Move IP right one byte
        // < : Move IP left one byte
        // + : increment value at IP
        // - : decrement value at IP
        // . : Output ASCII character stored in value at IP
        // , : Read ASCII character into value at IP
        // [ : Open loop; Enter if value at IP != 0, jump to end of loop otherwise
        // ] : Close loop; Loop back if value at IP != 0, continue otherwise
        //
        // Added characters: (for my sanity)
        // 0 : set value at IP to 0 (very useful)
        // A - Load value of register A into value at IP (IP = A)
        // a - Store index of IP in register A (A = IP)
        // $A - Move IP to index of value in register A (IP = *A)
        // $a - Store index of IP in register A (*A = IP)
        // *A - Load value of pointer in register A into value at IP (*IP = **A)
        // *a - Store value at IP into value of pointer in register A (**A = *IP)
        // & - Perform bitwise AND of value at IP and value of register A; store the result in value at IP 
        // | - Perform bitwise OR of value at IP and value of register A; store the result in value at IP 
        // ~ - Perform bitwise NOT of value at IP

        static void Main(string[] args)
        {
            byte[] cmd = new byte[1024];
            int count = 0, index = 0;
            byte A = 0;
            System.IO.StreamReader sr = new System.IO.StreamReader("C:\\Users\\Dustin\\Desktop\\test.bf");
            string str = sr.ReadToEnd();
            sr.Close();
            while(index < str.Length)
            {
                switch(str[index++])
                {
                    case '>':
                        if(count < 1023)
                        count++;
                        break;
                    case '<':
                        if(count > 0)
                        count--;
                        break;
                    case '+':
                        cmd[count]++;
                        break;
                    case '-':
                        cmd[count]--;
                        break;
                    case '.':
                        if((index < str.Length) && (str[index] == 'd'))
                        {
                            index++;
                            Console.Write(cmd[count].ToString());
                        }
                        else Console.Write((char)cmd[count]);
                        break;
                    case ',':
                        cmd[count] = (byte)Console.ReadLine()[0];
                       // Console.
                        break;
                    case '[':
                        if (cmd[count] == 0)
                            index = find_close_bracket(str, index);
                        break;
                    case ']':
                        if (cmd[count] != 0)
                            index = find_open_bracket(str, index);
                        break;
                    case '0':
                        cmd[count] = 0;
                        break;
                    case 'A': cmd[count] = A;
                        break;
                    case 'a': A = cmd[count];
                        break;
                    case '$'://A - Move IP to index of value in register A (IP = *A)
        // $a - Store index of IP in register A (*A = IP)
                    case '*'://A - Load value of pointer in register A into value at IP (*IP = **A)
        // *a - Store value at IP into value of pointer in register A (**A = *IP)
                    case '&':
                        cmd[count] &= A;
                        break;
                    case '|':
                        cmd[count] |= A;
                        break;
                    case '~':
                        cmd[count] = (byte)~cmd[count];
                        break;
                    case 'L':
                        cmd[count] <<= 1;
                        break;
                    case 'R':
                        cmd[count] >>= 1;
                        break;
                }
            }
            Console.ReadLine();
        }

        static int find_close_bracket(string s, int index)
        {
            int i = index;
            int num_brackets = 0;
            while(i < s.Length)
            {
                switch(s[i++])
                {
                    case '[':
                        num_brackets++;
                        break;
                    case ']':
                        if (num_brackets == 0) return i;
                        num_brackets--;
                        break;
                }
            }
            return s.Length;
        }

        static int find_open_bracket(string s, int index)
        {
            int i = index;
            int num_brackets = 0;
            while (i >= 0)
            {
                switch (s[--i])
                {
                    case ']':
                        num_brackets++;
                        break;
                    case '[':
                        num_brackets--;
                        if (num_brackets == 0) return i + 1;
                        break;
                }
            }
            return 0;
        }
    }
}

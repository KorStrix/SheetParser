using System.Collections.Generic;

namespace SpreadSheetParser
{
    public struct CommandLineArg
    {
        public string strArgName { get; private set; }
        public string strArgValue { get; private set; }

        public CommandLineArg(string strArgName)
        {
            this.strArgName = strArgName; this.strArgValue = "";
        }

        public CommandLineArg(string strArgName, string strArgValue)
        {
            this.strArgName = strArgName; this.strArgValue = strArgValue;
        }
    }

    class CommandLineParser
    {
        public enum Error
        {
            HasNot_StartWith_Minus,
            EmptyValue,
            Is_Not_CommandLineArg,
        }

        public delegate bool delOnCheck_IsValid(string strCommandLineArgName, out bool bHasValue);
        public delegate void delOnParsingError(string strParsingText, Error eError);

        public static List<CommandLineArg> Parsing_CommandLine(string strCommandLineText, delOnCheck_IsValid OnCheck_IsValid, delOnParsingError OnParsingError = null)
        {
            List<CommandLineArg> listCommandLine = new List<CommandLineArg>();
            if (string.IsNullOrEmpty(strCommandLineText))
                return listCommandLine;

            if (OnParsingError == null)
                OnParsingError = OnParsingError_Default;

            string[] arrCommandArgs = strCommandLineText.Split(' ');
            for(int i = 0; i < arrCommandArgs.Length; i++)
            {
                string strCommandArg = arrCommandArgs[i];
                if(strCommandArg.StartsWith("-"))
                {
                    strCommandArg = strCommandArg.Remove(0, 1);
                    string strNextText = "";
                    if (i != arrCommandArgs.Length - 1)
                        strNextText = arrCommandArgs[i + 1];

                    bool bHasValue;
                    if(OnCheck_IsValid(strCommandArg, out bHasValue) == false)
                    {
                        OnParsingError(strCommandArg, Error.Is_Not_CommandLineArg);
                        continue;
                    }

                    if (bHasValue)
                    {
                        if(string.IsNullOrEmpty(strNextText))
                            OnParsingError(strCommandArg, Error.EmptyValue);

                        listCommandLine.Add(new CommandLineArg(strCommandArg, strNextText));
                        i++;
                    }
                    else
                    {
                        listCommandLine.Add(new CommandLineArg(strCommandArg));
                    }
                }
                else
                {
                    OnParsingError(strCommandArg, Error.HasNot_StartWith_Minus);
                }
            }

            return listCommandLine;
        }

        private static void OnParsingError_Default(string strParsingText, Error eError)
        {
        }
    }
}

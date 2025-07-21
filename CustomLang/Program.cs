using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Antlr4Ast;
using ApexParser;
using ApexParser.MetaClass;
using ApexParser.Visitors;
using CustomLang.Config.Parser;
using CustomLang.Config.Parser.Decorators.IniParserDecorator;
using CustomLang.Config.Parser.Settings;
using IniParser;
using Sprache;
using CustomLang;
using CustomLang.Interpreter;
using CustomLang.Interpreter.AstNodes;
using CustomLang.Interpreter.AstNodes.Types;
using CustomLang.Interpreter.config;
using CustomLang.Interpreter.Config;
using CustomLang.Interpreter.Debug;
using CustomLang.Interpreter.Exceptions;
using CustomLang.Interpreter.Extentions;
using CustomLang.Interpreter.interfaces;
using CustomLang.Interpreter.Visitors;
using CustomLang.Interpreter.Visitors.LangObj;
using CustomLang.Storage;
using CustomLang.Storage.Trie;
using Microsoft.Extensions.Options;
using ExecutionContext = CustomLang.Interpreter.Debug.ExecutionContext;


class Program
{
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_";
    
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Config.Interpreterexe <input-file>");
            return -1;
        }
        
        var arguments = new Arguments();
        
        for (int i = 0; i < args.Length; i++)
        {
            var isValueArg = i < args.Length - 1;
            switch (args[i])
            {
                case "--debug": case "-d": case "/debug":
                    arguments.DebugMode = true;
                    break;
                case "--config-file" when isValueArg:
                    arguments.ConfigFile = args[++i];
                    break;
                case "--execute-file" when isValueArg:
                    arguments.InputFile = args[++i];
                    break;
                case "--base-assign" when isValueArg:
                    arguments.BaseAssign = int.Parse(args[++i]);
                    break;
                case "--base-input" when isValueArg:
                    arguments.BaseInput = int.Parse(args[++i]);
                    break;
                case "--base-output" when isValueArg:
                    arguments.BaseOutput = int.Parse(args[++i]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown argument: " + args[i]);
            }
        }
        
        var fileParser = new CommentIniParser(
            new FileIniDataParser());
        
        var interpreterConfig = new InterpreterConfigBuilder()
            .SetInputBase(arguments.BaseInput)
            .SetOutputBase(arguments.BaseOutput)
            .SetAssignmentBase(arguments.BaseAssign)
            .SetDebugMode(arguments.DebugMode)
            .Build();
   
        try
        {
            
            ITrieFactory factory = new TrieFactory(Alphabet);
            
            var storage = 
               new TrieStorage<LangObject>(factory.Create<LangObject>());
            
            var storageFunctions = 
                new TrieStorage<FunctionStatementNode>(
                factory.Create<FunctionStatementNode>());
            
            IOutputWriter writer = new ConsoleOutputWriter();

            IInputReader consoleReader = new ConsoleInputReader();

            IIOContext ioContext = new InputOutputContext(writer, consoleReader);
            
            IStorageContext storageContext = 
                new ExecutionContext(storage, storageFunctions);
            
            var configGrammar = fileParser.LoadFromIni<CustomLangGrammarConfig>(
                arguments.ConfigFile);
            
            var parser = new CustomLangParser(
                new CustomLangGrammar(configGrammar));

            ICustomLangDebug debug = 
                new CustomDebugger(storageContext, ioContext);
            
            var visitor = new InterpreterCodeVisitor(
                parser,
                storageContext,
                ioContext,
                interpreterConfig, 
                functionsList: null,
                debug: debug);
            
            var interpreter = new CustomLangInterpreter(
                parser,
                visitor);
            
            var builder = new StringBuilder();
          
            using (var reader = new StreamReader(arguments.InputFile))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    builder.AppendLine(line);
                }
            }
            
            await interpreter.ParseAsync(builder.ToString());
        
            builder.Clear();
        
        }
        catch (CustomLangException e)
        {
            Console.WriteLine(
                              $"Parse error: in line {e.LineNumber}, " +
                              $"position {e.ColumnPosition}:\n" +
                              $"Message: {e.Message}\n");
            return -1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return -2;
        }
        
        return 0;
        
    }
    class Arguments
    {
        public string ConfigFile { get; set; }
        public string InputFile { get; set; }
        public bool DebugMode { get; set; }
        public int BaseAssign { get; set; }
        public int BaseInput { get; set; }
        public int BaseOutput { get; set; }
    }
    
}

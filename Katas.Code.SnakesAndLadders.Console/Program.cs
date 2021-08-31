using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Katas.Code.SnakesAndLadders.Domain.Contrats;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using Katas.Code.SnakesAndLadders.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Katas.Code.SnakesAndLadders.Console
{
    class Program
    {
        private static IConfiguration _config;
        private static IDiceService _diceService;
        private static ITokenService _tokenService;
        private static IGameService _gameService;

        private static List<Token> _tokens;
        private static Token _token;

        private static List<Option> _options;
        private static int _numberOfPlayers;
        private static int _index = 0;
        private static bool _gameStatus = false;

        static void Main(string[] args)
        {
            var host = AppStartup();

            var dice = new Dice()
            {
                MinValue = _config.GetValue<int>("Dice:Min"),
                MaxValue = _config.GetValue<int>("Dice:Max")
            };

            var board = new Board()
            {
                Guid = Guid.NewGuid(),
                Size = _config.GetValue<int>("Game:Size"),
                Snakes = _config.GetSection("Game:Snakes").Get<Dictionary<string, int>>(),
                Ladders = _config.GetSection("Game:Ladders").Get<Dictionary<string, int>>()
            };

            _diceService = ActivatorUtilities.CreateInstance<DiceService>(host.Services, dice);
            _tokenService = ActivatorUtilities.CreateInstance<TokenService>(host.Services);
            _gameService = ActivatorUtilities.CreateInstance<GameService>(host.Services, board);

            _options = new List<Option>
            {
                new Option("Players", InputNumberOfPlayers),
                new Option("Start Game", StartGame),
                new Option("Exit", () => Environment.Exit(0)),
            };

            WriteMenu(_options, _options[_index]);

            ConsoleKeyInfo keyinfo;

            do
            {
                keyinfo = System.Console.ReadKey();

                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (_index + 1 < _options.Count)
                    {
                        _index++;
                        WriteMenu(_options, _options[_index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (_index - 1 >= 0)
                    {
                        _index--;
                        WriteMenu(_options, _options[_index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    _options[_index].Selected.Invoke();
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            System.Console.ReadKey();

        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            _config = builder.Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ITokenService, TokenService>()
                        .AddTransient<IDiceService, DiceService>()
                        .AddTransient<IGameService, GameService>();
                })
                .Build();

            return host;
        }

        static void InputNumberOfPlayers()
        {
            System.Console.Clear();
            Header();

            System.Console.WriteLine("\t# Input the number of     #");
            System.Console.WriteLine("\t# players:                #");
            System.Console.WriteLine("\t#                         #");

            Footer();

            System.Console.SetCursorPosition(10, 8);
            var numberOfPlayers = System.Console.ReadLine();
            if (Int32.TryParse(numberOfPlayers, out _numberOfPlayers))
            {
                _index = 1;
                WriteMenu(_options, _options[_index]);
            }
            else
            {
                InputNumberOfPlayers();
            }
        }

        static void StartGame()
        {
            _tokens = _tokenService.GetListTokens();

            if (_tokens.Count == 0)
            {
                if (_numberOfPlayers > 0)
                {
                    for (int i = 0; i < _numberOfPlayers; i++)
                    {
                        _tokens.Add(_tokenService.CreateToken());
                    }
                    _gameStatus = _gameService.Start();
                    GameRunning();
                }
                else
                {
                    _index = 0;
                    WriteMenu(_options, _options[_index]);
                }
            }
        }

        static void GameRunning()
        {

            for (int i = 1; i <= _tokens.Count; i++)
            {
                if (_gameStatus)
                {
                    CurrentPosition(i);
                }
                else
                {
                    break;
                }
            }
            if (_gameStatus)
                GameRunning();
        }

        static void CurrentPosition(int player)
        {
            System.Console.Clear();
            Header();
            System.Console.WriteLine("\t# Player {0} your torn      #", player);
            System.Console.Write("\t# Your position is {0}", _tokens[player - 1].Position);
            var lengthPosition = _tokens[player - 1].Position.ToString().Length;
            for (int i = 1; i < 8 - lengthPosition; i++)
            {
                System.Console.Write(" ");
            }
            System.Console.WriteLine("#");
            System.Console.WriteLine("\t# Press enter key...      #");
            Footer();
            System.Console.ReadLine();
            NewPosition(player);
        }

        static void NewPosition(int player)
        {
            System.Console.Clear();
            Header();
            System.Console.WriteLine("\t# Player {0}                #", player);
            var newDiceNumber = _diceService.GetRandomNumber();
            var newPosition = _gameService.NewBoxGameAfterSnakesAndLadders(_tokens[player - 1].Position, newDiceNumber);
            System.Console.Write("\t# Dice: {0}", newDiceNumber);
            var dice = newDiceNumber.ToString().Length;
            for (int i = 1; i < 19 - dice; i++)
            {
                System.Console.Write(" ");
            }
            System.Console.WriteLine("#");
            System.Console.WriteLine("\t# Your new position       #");
            System.Console.Write("\t# is {0}", newPosition);
            var isPosition = newPosition.ToString().Length;
            for (int i = 1; i < 22 - isPosition; i++)
            {
                System.Console.Write(" ");
            }
            System.Console.WriteLine("#");
            _tokenService.UpdateTokenPosition(_tokens[player - 1], newPosition);
            System.Console.WriteLine("\t# Press enter key...      #");
            Footer();
            System.Console.ReadLine();
            if (newPosition >= _gameService.GetNumberOfBoxesGames())
            {
                EndGame(player);
            }
        }

        static void EndGame(int player)
        {
            System.Console.Clear();
            Header();
            System.Console.WriteLine("\t# The player {0}            #", player);
            System.Console.WriteLine("\t# is the winner           #");
            System.Console.WriteLine("\t# ----------------------- #");
            System.Console.WriteLine("\t# Press enter key...      #");
            Footer();
            System.Console.ReadLine();
            _gameStatus = _gameService.Stop();
            _index = 0;
            WriteMenu(_options, _options[_index]);
        }

        static void WriteMenu(List<Option> Options, Option selectedOption)
        {
            System.Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.White;

            Header();

            foreach (Option option in Options)
            {
                if (option == selectedOption)
                {
                    System.Console.Write("\t# > ");
                }
                else
                {
                    System.Console.Write("\t#   ");
                }

                System.Console.Write(option.Name);
                var lengthOption = option.Name.Length;
                for (int i = 1; i < 23 - lengthOption; i++)
                {
                    System.Console.Write(" ");
                }
                System.Console.WriteLine("#");
            }

            Footer();
        }

        private static void Header()
        {
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("\t###########################");
            System.Console.WriteLine("\t# Snakes And Ladders Game #");
            System.Console.WriteLine("\t# ----------------------- #");
            System.Console.WriteLine("\t#                         #");
        }
        private static void Footer()
        {
            System.Console.WriteLine("\t#                         #");
            System.Console.WriteLine("\t# ----------------------- #");
            System.Console.WriteLine("\t#   Thank you for play    #");
            System.Console.WriteLine("\t###########################");
        }

    }

    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }

}

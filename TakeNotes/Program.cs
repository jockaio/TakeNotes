using Microsoft.CognitiveServices.Speech;
using SpeechRecognitionService;
using System;

namespace TakeNotes
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async System.Threading.Tasks.Task MainAsync(string[] args)
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key // and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("10cddabebc8c44228e36f8bbebd3f16f", "NorthEurope");

            var recognizer = new SpeechRecognizer(config);

            var speechService = new SpeechService(recognizer);

            await speechService.StartSpeechRecognitionAsync();

            Console.ReadLine();

            var conversation = await speechService.StopSpeechRecognition();

            Console.WriteLine("Conversation was: ");
            foreach (var speechBit in conversation)
            {
                Console.WriteLine($"- {speechBit.Result}");
            }

            Console.ReadLine();
        }
    }
}
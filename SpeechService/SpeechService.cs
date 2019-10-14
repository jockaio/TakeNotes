using Microsoft.CognitiveServices.Speech;
using SpeechRecognitionService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeechRecognitionService
{
    public class SpeechService
    {
        public SpeechRecognizer _speechRecognizer { get; set; }
        internal List<ReconizedSpeech> _reconizedSpeeches { get; private set; }

        public SpeechService(SpeechRecognizer speechRecognizer)
        {
            _speechRecognizer = speechRecognizer;
            _reconizedSpeeches = new List<ReconizedSpeech>();
        }

        public async Task StartSpeechRecognitionAsync()
        {
            _speechRecognizer.SpeechStartDetected += _speechRecognizer_SpeechStartDetected;
            _speechRecognizer.Recognized += _speechRecognizer_Recognized;
            _speechRecognizer.SpeechEndDetected += _speechRecognizer_SpeechEndDetected;
            await _speechRecognizer.StartContinuousRecognitionAsync();
        }

        private void _speechRecognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            Console.WriteLine($"---- End of taking notes. ----");
        }

        public async Task<List<ReconizedSpeech>> StopSpeechRecognition()
        {
            await _speechRecognizer.StopContinuousRecognitionAsync();
            _speechRecognizer.Dispose();
            return _reconizedSpeeches;
        }

        private void _speechRecognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            var result = e.Result;

            // Checks result.
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                if (!String.IsNullOrWhiteSpace(result.Text))
                {
                    Console.WriteLine($"- {result.Text}");
                    _reconizedSpeeches.Add(
                        new ReconizedSpeech
                        {
                            Result = result.Text
                        });
                }
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
                }
            }
        }

        private void _speechRecognizer_SpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            Console.WriteLine("---- Taking notes ----");
        }
    }
}
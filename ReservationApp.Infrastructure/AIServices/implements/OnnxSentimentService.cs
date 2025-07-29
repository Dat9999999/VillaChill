using System.Text.RegularExpressions;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using ReservationApp.Application.Services.interfaces;

public class OnnxSentimentService : IOnnxSentimentService
{
    private readonly InferenceSession _session;
    private readonly Dictionary<string, int> _vocab;

    public OnnxSentimentService(string modelPath, string vocabPath)
    {
        _session = new InferenceSession(modelPath);
        _vocab = LoadVocab(vocabPath);
    }

    public string Predict(string text)
    {
        var tokens = Tokenize(text);
        var inputIds = new DenseTensor<long>(new[] { 1, tokens.Length });

        for (int i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            inputIds[0, i] = _vocab.TryGetValue(token, out var id) ? id : _vocab["[UNK]"];
        }

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIds)
        };

        using var results = _session.Run(inputs);
        var scores = results.First().AsEnumerable<float>().ToArray();

        return scores[1] > scores[0] ? "POSITIVE" : "NEGATIVE";
    }

    private Dictionary<string, int> LoadVocab(string path)
    {
        var vocab = new Dictionary<string, int>();
        var lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length; i++)
        {
            vocab[lines[i]] = i;
        }
        return vocab;
    }

    private string[] Tokenize(string text)
    {
        text = text.ToLower();
        text = Regex.Replace(text, @"[^\w\s]", "");
        var words = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        return new[] { "[CLS]" }.Concat(words).Append("[SEP]").ToArray();
    }
}
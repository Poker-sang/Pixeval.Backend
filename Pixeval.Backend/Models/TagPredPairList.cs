namespace Pixeval.Backend.Models;

public record TagPredPairList(Dictionary<string, float> Rating, Dictionary<string, float> CharacterRes, Dictionary<string, float> GeneralRes);

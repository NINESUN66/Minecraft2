using UnityEngine;

public class ExpAttribute : FollowPlayer
{
    public float Experience;
    private readonly float[] experienceWeights = { 0.3f, 0.5f, 0.3f, 0.15f, 0.05f }; // 经验球的权重

    private void Start()
    {
        int randomImage = GetWeightedRandomIndex(experienceWeights);
        Experience = GenerateExperience(randomImage);
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EXP/" + randomImage);
    }

    private int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i + 1;
            }
        }

        return 1;
    }

    private float GenerateExperience(int randomIndex)
    {
        switch (randomIndex)
        {
            case 1:
                return Random.Range(10f, 20f);
            case 2:
                return Random.Range(20f, 30f);
            case 3:
                return Random.Range(30f, 40f);
            case 4:
                return Random.Range(40f, 50f);
            case 5:
                return Random.Range(50f, 55f);
            default:
                return 10f;
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}


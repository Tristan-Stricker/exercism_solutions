static class QuestLogic
{
    public static bool CanFastAttack(bool knightIsAwake)
    {
        return knightIsAwake == false;
    }

    public static bool CanSpy(bool knightIsAwake, bool archerIsAwake, bool prisonerIsAwake)
    {
        return knightIsAwake || archerIsAwake || prisonerIsAwake;
}

    public static bool CanSignalPrisoner(bool archerIsAwake, bool prisonerIsAwake)
    {
       var archerIsSleeping = archerIsAwake == false;
       return prisonerIsAwake && archerIsSleeping;
    }

    public static bool CanFreePrisoner(
        bool knightIsAwake,
        bool archerIsAwake, 
        bool prisonerIsAwake,
        bool petDogIsPresent)
    {
        var archerIsAsleep = archerIsAwake == false;
        var knightIsAsleep = knightIsAwake == false;
        return petDogIsPresent && archerIsAsleep ||
               !petDogIsPresent && prisonerIsAwake && archerIsAsleep && knightIsAsleep;
    }
}

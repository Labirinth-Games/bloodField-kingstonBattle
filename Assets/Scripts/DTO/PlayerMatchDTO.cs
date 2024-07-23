using BloodField.Enums;

namespace BloodField.DTO
{
    [System.Serializable]
    public class PlayerMatchDTO
    {
        public string userId;

        public PhaseEnum turnStage = PhaseEnum.Preparation;
        public bool isFinishPreparation = false; // when player finish stage preparation

        // gameplay
        public int amountUsedCards = 0;
        public bool isAllMiniatureFinishActions = false;
        public bool isMyTurn = false;

        public void Reset()
        {
            amountUsedCards = 0;
            isAllMiniatureFinishActions = false;
        }
    }
}
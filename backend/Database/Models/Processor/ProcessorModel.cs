namespace Database.Models.Processor
{
    public class ProcessorModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int InputItem { get; set; }
        public int InputStepAmount { get; set; }
        public int OutputItem { get; set; }
        public int OutputStepAmount { get; set; }
        public int Time { get; set; }
        public uint PedModel { get; set; }

        public ProcessorModel() { }

        public ProcessorModel(int positionId, int inputItem, int inputStepAmount, int outputItem, int outputStepAmount, int time, uint pedModel)
        {
            PositionId = positionId;
            InputItem = inputItem;
            InputStepAmount = inputStepAmount;
            OutputItem = outputItem;
            OutputStepAmount = outputStepAmount;
            Time = time;
            PedModel = pedModel;
        }
    }
}
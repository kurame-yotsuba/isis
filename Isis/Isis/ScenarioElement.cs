namespace Isis
{
	class ScenarioElement
	{
		public Command Command { get; }
		public string Text { get; }

		public ScenarioElement(Command command, string text)
		{
			Command = command;
			Text = text;
		}

		public override string ToString() => $"{Command.Name}\t{Text}";
	}
}

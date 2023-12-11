using AltV.Net.Data;

namespace Core.Extensions
{
	public static class PositionExtensions
	{
		public static Position Down(this Position position, float offset = 1)
		{
			return new(position.X, position.Y, position.Z - offset);
		}
	}
}
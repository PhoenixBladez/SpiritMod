using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Players
{
	/// <summary>
	/// ModPlayer class managing extra additive or alphablend draw calls on top of a player, due to the inflexibility of playerlayers.
	/// </summary>
	public class ExtraDrawOnPlayer : ModPlayer
	{
		public enum DrawType
		{
			AlphaBlend,
			Additive
		}

		public delegate void DrawAction(SpriteBatch spriteBatch);

		public IDictionary<DrawAction, DrawType> DrawDict = new Dictionary<DrawAction, DrawType>();

		public override void ResetEffects() => DrawDict = new Dictionary<DrawAction, DrawType>();

		/// <summary>
		/// Check if any of the draw calls on the player have the specified draw type.
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>
		public bool AnyOfType(DrawType Type)
		{
			foreach (KeyValuePair<DrawAction, DrawType> kvp in DrawDict)
				if (kvp.Value == Type)
					return true;

			return false;
		}

		/// <summary>
		/// Draw all draw calls on the player with a specified draw type.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="Type"></param>
		public void DrawAllCallsOfType(SpriteBatch spriteBatch, DrawType Type)
		{
			foreach (KeyValuePair<DrawAction, DrawType> kvp in DrawDict)
				if (kvp.Value == Type)
					kvp.Key.Invoke(spriteBatch);
		}

		/// <summary>
		/// Static method called in a detour after drawing all players.<br />
		/// Iterates through all players, adding the extra calls to a list, then draws them in seperate batches, if any players have a call of the type.
		/// </summary>
		public static void DrawPlayers()
		{
			List<ExtraDrawOnPlayer> additiveCallPlayers = new List<ExtraDrawOnPlayer>();
			List<ExtraDrawOnPlayer> alphaBlendCallPlayers = new List<ExtraDrawOnPlayer>();
			foreach (Player player in Main.player.Where(x => x.active && x != null))
			{
				if (player.GetModPlayer<ExtraDrawOnPlayer>().AnyOfType(DrawType.Additive))
					additiveCallPlayers.Add(player.GetModPlayer<ExtraDrawOnPlayer>());

				if (player.GetModPlayer<ExtraDrawOnPlayer>().AnyOfType(DrawType.AlphaBlend))
					alphaBlendCallPlayers.Add(player.GetModPlayer<ExtraDrawOnPlayer>());
			}

			if (additiveCallPlayers.Any())
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
				foreach (ExtraDrawOnPlayer player in additiveCallPlayers)
					player.DrawAllCallsOfType(Main.spriteBatch, DrawType.Additive);
				Main.spriteBatch.End();
			}

			if (alphaBlendCallPlayers.Any())
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
				foreach (ExtraDrawOnPlayer player in alphaBlendCallPlayers)
					player.DrawAllCallsOfType(Main.spriteBatch, DrawType.AlphaBlend);
				Main.spriteBatch.End();
			}
		}
	}
}
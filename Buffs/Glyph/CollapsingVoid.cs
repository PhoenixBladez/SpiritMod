using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
	public class CollapsingVoid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Collapsing Void");
			Description.SetDefault("");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			if (time >= 60) {
				MyPlayer modPlayer = player.GetSpiritPlayer();
				if (modPlayer.voidStacks < 3) {
					modPlayer.voidStacks++;
				}

				Main.buffNoTimeDisplay[Type] = false;
			}

			return false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			player.endurance += modPlayer.voidStacks * 0.05f;

			if (modPlayer.voidStacks > 1 && player.buffTime[buffIndex] <= 2) {
				modPlayer.voidStacks--;
				player.buffTime[buffIndex] = 299;
			}

			if (modPlayer.voidStacks <= 1) {
				Main.buffNoTimeDisplay[Type] = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			MyPlayer modPlayer = Main.LocalPlayer.GetSpiritPlayer();
			var texture = Mod.Assets.Request<Texture2D>("Buffs/Glyph/CollapsingVoid_" + (modPlayer.voidStacks - 1)).Value;
			if (modPlayer.divineStacks == 1)
				texture = Mod.Assets.Request<Texture2D>("Buffs/Glyph/CollapsingVoid").Value;

			spriteBatch.Draw(texture, drawParams.Position, drawParams.DrawColor);
			return false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.LocalPlayer.GetSpiritPlayer();
			tip = $"Damage taken is reduced by {modPlayer.voidStacks * 5}%";
			rare = modPlayer.voidStacks >> 1;
		}
	}
}

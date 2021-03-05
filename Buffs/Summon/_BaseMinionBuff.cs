using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class BaseMinionBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture) => false;
		public Player Player => Main.LocalPlayer;
		public string displayName = "big";
		public string description = "chungus";
		public bool MinionFlag = true;
		public BaseMinionBuff(string displayName, string description)
		{
			this.displayName = displayName;
			this.description = description;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("big");
			Description.SetDefault("chung");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		/*public override void Update(Player player, ref int buffIndex)
		{
			MinionFlag = false;
			if (CheckMinionType()) {
				MinionFlag = true;
			}
			CheckMinionFlag();

			if (!MinionFlag) {
				//player.DelBuff(buffIndex);
				//buffIndex--;
				//return;
			}

			player.buffTime[buffIndex] = 18000;
		}*/

		public virtual bool CheckMinionType() => true;

		public virtual void CheckMinionFlag() { }
	}
}

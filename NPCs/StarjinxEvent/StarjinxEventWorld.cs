using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.IO;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class StarjinxEventWorld : ModWorld
    {
        public static bool StarjinxActive = false;
        public static bool SpawnedStarjinx = false;

		//Synced between clients, used to calculate other variables and for drawing the ui
		public static int MaxEnemies = 0;
		public static int KilledEnemies = 0;
		public static int CometsRemaining = 0;

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"Starjinx Active?", StarjinxActive}
            };
        }

		public override void Initialize()
		{
			MaxEnemies = 0;
			KilledEnemies = 0;
			CometsRemaining = 0;

			StarjinxUI.Initialize();
		}

		public override void PostUpdate() => StarjinxUI.Update();

		public static void SetMaxEnemies(int maxEnemies)
		{
			MaxEnemies = maxEnemies;
			KilledEnemies = 0;
			SendInfoPacket();
		}

		public static void IncrementKilledEnemies()
		{
			KilledEnemies++;
			KilledEnemies = (int)MathHelper.Clamp(KilledEnemies, 0, MaxEnemies);
			SendInfoPacket();
		}

		public static void SetComets(int cometAmount)
		{
			CometsRemaining = cometAmount;

			SendInfoPacket();
		}

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("Starjinx Active?"))
                StarjinxActive = tag.GetBool("Starjinx Active?");
            SpawnedStarjinx = false;
        }

		//Edited from TideWorld's netcode handling
		public static ModPacket CreateProgressPacket()
		{
			ModPacket packet = ModContent.GetInstance<SpiritMod>().GetPacket();
			packet.Write((byte)MessageType.StarjinxData);
			packet.Write(MaxEnemies);
			packet.Write(KilledEnemies);
			packet.Write(CometsRemaining);

			return packet;
		}

		public static void SendInfoPacket()
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				return;

			CreateProgressPacket().Send();
		}

		public static void HandlePacket(BinaryReader reader)
		{
			MaxEnemies = reader.ReadInt32();
			KilledEnemies = reader.ReadInt32();
			CometsRemaining = reader.ReadInt32();

			if (Main.netMode == NetmodeID.Server)
				SendInfoPacket(); // Forward packet to rest of clients
		}
	}
}
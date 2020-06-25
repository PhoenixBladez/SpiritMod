using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Weapon.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class VileWasp : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Pesterfly");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults() {
            npc.width = 22;
            npc.height = 20;
            npc.damage = 20;
            npc.defense = 0;
            npc.lifeMax = 14;
            npc.HitSound = SoundID.NPCHit7; //Dr Man Fly
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 10f;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.knockBackResist = .65f;
            npc.aiStyle = 44;
            aiType = NPCID.FlyingAntlion;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
		public override void AI()
        {
            npc.spriteDirection = npc.direction;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Pokey
{
    internal class Pokey_Body : ModNPC
    {
        private int segments = 15;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pokey");
            Main.npcFrameCount[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 16;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.value = 
            npc.lifeMax = 50;
            npc.damage = 10;
            npc.defense = 4;
            npc.noTileCollide = false;
            npc.dontCountMe = true;
            segments = Main.rand.Next(5, 8) + (Main.expertMode ? 2 : 0);

            if (Main.rand.Next(2) == 0)
            {
                npc.frame.Y = 32;
            }
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = 75;

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{

			if (Main.tileSand[spawnInfo.spawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 0.35f;
			return 0;
		}
        private int UpperChain {
			get => (int)npc.ai[0];
			set => npc.ai[0] = value;
		}
		private int LowerChain {
			get => (int)npc.ai[1];
			set => npc.ai[1] = value;
		}
		private int QueueFromBottom()
        {
            int ret = 0;
            int bottomChain = LowerChain;
            while (bottomChain != -1 && ret <= segments * 2)
            {
                ret++;
                if (!Main.npc[bottomChain].active)
                    break;
                bottomChain = (int)Main.npc[bottomChain].ai[1];
            }
            return ret;
        }
        private int QueueFromTop()
        {
            int ret = 0;
            int topChain = UpperChain;
            while (topChain != -1 && ret <= segments * 2)
            {
                ret++;
                if (!Main.npc[topChain].active)
                    break;
                topChain = (int)Main.npc[topChain].ai[0];
            }
            return ret;
        }
        private NPC Tail
        {
            get
            {
                int tries = 0;
                int bottomChain = npc.whoAmI;
                while (Main.npc[bottomChain].ai[1] != -1)
                {
                    if (!Main.npc[bottomChain].active)
                        break;
                    bottomChain = (int)Main.npc[bottomChain].ai[1];
                    if (tries++ > segments * 2)
                        break;
                }
                return Main.npc[bottomChain];
            }
            set
            {
                int tries = 0;
                 int bottomChain = Head.whoAmI;
                while (Main.npc[bottomChain] != value)
                {
                    bottomChain = (int)Main.npc[bottomChain].ai[1];
                    if (Main.npc[bottomChain].ai[1] == -1)
                    {
                        return;
                    }
                     if (tries++ > segments * 2)
                        break;
                }
                Main.npc[bottomChain].ai[1] = -1;
            }
        }
        private NPC Head
        {
            get
            {
                int tries = 0;
                int topChain = npc.whoAmI;
                while (Main.npc[topChain].ai[0] != -1)
                {
                    if (!Main.npc[topChain].active)
                        break;
                    topChain = (int)Main.npc[topChain].ai[0];
                    if (tries++ > segments * 2)
                        break;
                }
                return Main.npc[topChain];
            }
            set
            {
                 int topChain = Tail.whoAmI;
                while (Main.npc[topChain] != value)
                {
                    topChain = (int)Main.npc[topChain].ai[0];
                    if (Main.npc[topChain].ai[0] == -1)
                    {
                        return;
                    }
                }
                Main.npc[topChain].ai[0] = -1;
            }
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            if (LowerChain == 0 && UpperChain == 0)   
            {
                LowerChain = -1;
                int latestSegment = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y - npc.height, ModContent.NPCType<Pokey_Body>(), 0, -1, npc.whoAmI);
                UpperChain = latestSegment;
                for (int i = 2; i < segments; i++)
                {

                    Main.npc[latestSegment].ai[0] = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y - (i * npc.height), ModContent.NPCType<Pokey_Body>(), 0,-1, latestSegment);
                    latestSegment = (int)Main.npc[latestSegment].ai[0];
                }
            }
            if (!Head.active || Head.life <= 0)
            {
                npc.life = 0;
				NPCLoot();
                return;
            }
            int queue = QueueFromBottom();
            if (queue == 0)
            {
                npc.noGravity = false;
                //ai stuff for base goes here
                npc.velocity.X += Math.Sign(Main.player[npc.target].Center.X - npc.Center.X) / 10f;
                npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -2, 2);
                npc.spriteDirection = -npc.direction;
                Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);
                CheckPlatform(Main.player[npc.target]);
                CheckPit();
            }
            else
            {
                npc.ai[2] += 0.15f;
                npc.noGravity = true;
                npc.aiStyle = -1;
                npc.direction = Tail.direction;
                npc.spriteDirection = -npc.direction;
                npc.position.X = Tail.position.X + ((float)Math.Sin(queue + npc.ai[2]) * 3);
                float posToBe = Main.npc[LowerChain].position.Y - (npc.height);
                if (QueueFromTop() == 0)
                    posToBe -= 10;
                GoTo(posToBe);
            }
            if (UpperChain != -1)
            {
                if (!Main.npc[UpperChain].active || Main.npc[UpperChain].life <= 0)
                {
                    UpperChain = (int)Main.npc[UpperChain].ai[0];
                }
            }
            if (LowerChain != -1)
            {
                if (!Main.npc[LowerChain].active || Main.npc[LowerChain].life <= 0)
                {
                    LowerChain = (int)Main.npc[LowerChain].ai[1];
                }
            }
            if (QueueFromTop() == 0)
            {
                npc.frame.Y = 64;
            }
        }
        private void CheckPlatform(Player player)
        {
            bool onplatform = true;
            for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4)
            {
                Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
                if (!TileID.Sets.Platforms[tile.type])
                    onplatform = false;
            }
            if (onplatform && (npc.Center.Y < player.position.Y - 20))
                npc.noTileCollide = true;
            else
                npc.noTileCollide = false;
        }
        private void CheckPit()
        {
            bool pit = true;
            for (int i = 1; i <= 4; i++)
            {
                Tile forwardtile = Framing.GetTileSafely(new Point((int)(npc.Center.X / 16) + Math.Sign(npc.velocity.X), (int)(npc.Center.Y / 16) + i));
                if (WorldGen.SolidTile(forwardtile) || WorldGen.SolidTile2(forwardtile) || WorldGen.SolidTile3(forwardtile))
                    pit = false;
            }
            if (pit)
                npc.velocity.X *= -0.5f;
        }
        private void GoTo(float pos) 
        {
            float lerpspeed = (Math.Abs(npc.position.Y - pos) > (npc.height / 2)) ? 0.45f : 0.15f;
            npc.position.Y = MathHelper.Lerp(npc.position.Y, pos, lerpspeed); 
        }
        private void Delete()
        {
            if (QueueFromTop() == 0)
                return;
            if (QueueFromBottom() == 0)
            {
                Tail = Main.npc[UpperChain];
                return;
            }
            int chain = Head.whoAmI;
            int tries = 0;
            while (Main.npc[chain].ai[1] != npc.whoAmI)
            {
                chain = (int)Main.npc[chain].ai[1];
                if (chain == -1)
                    return;
                if (tries++ > segments * 2 || Main.npc[chain].ai[1] == -1)
                     break;
            }
            Main.npc[chain].ai[1] = LowerChain;

            chain = Tail.whoAmI;
            tries = 0;
            while (Main.npc[chain].ai[0] != npc.whoAmI)
            {
                chain = (int)Main.npc[chain].ai[0];
                if (chain == -1)
                    return;
                if (tries++ > segments * 2 || Main.npc[chain].ai[0] == -1)
                    break;
            }
            Main.npc[chain].ai[0] = UpperChain;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
            Main.PlaySound(SoundID.Dig, (int)npc.Center.X, (int)npc.Center.Y, 1, 0.5f, 0.25f);

            Tail.velocity.X = hitDirection * 2;
        }

		public override bool CheckDead()
		{
			try {
				if (QueueFromBottom() != 0 || (Head.active && Head.life > 0))
					Delete();
			}
			catch (System.Exception) {
				throw new Exception("It's in delete number " + QueueFromBottom().ToString());
			}
			return true;
		}

		public override void NPCLoot()
        {
            if(Head.active && Head.life > 0) 
            {//no overlapping death sfx on head kill
                Main.PlaySound(SoundID.Dig, (int)npc.Center.X, (int)npc.Center.Y, 1, 1f, -0.25f);
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Cactus, Main.rand.Next(2, 4) + 4);
                if (Main.rand.NextBool(5))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PinkPricklyPear);
                }
            }
            switch (npc.frame.Y)
            {
                case 0:
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PokeyGores/Pokey1_Gore1"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PokeyGores/Pokey1_Gore2"), 1f);
                    break;
                case 32:
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PokeyGores/Pokey2_Gore1"), 1f);
                    break;
                case 64:
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PokeyGores/PokeyHead_Gore1"), 1f);
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PokeyGores/PokeyHead_Gore2"), 1f);
                    break;
            }
        }
    }
}

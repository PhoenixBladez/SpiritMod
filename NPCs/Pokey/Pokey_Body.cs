using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.Pokey
{
    internal class Pokey_Body : ModNPC
    {
        private int segments = 15;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stactus");
            Main.npcFrameCount[NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 16;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 70;
            NPC.damage = 10;
            NPC.defense = 4;
            NPC.noTileCollide = false;
            NPC.dontCountMe = true;
            segments = Main.rand.Next(5, 8) + (Main.expertMode ? 2 : 0);

            if (Main.rand.Next(2) == 0)
            {
                NPC.frame.Y = 32;
            }
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.PokeyBanner>();
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = 95;

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{

			if (Main.tileSand[spawnInfo.SpawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 0.38f;
			return 0;
		}
        private int UpperChain {
			get => (int)NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private int LowerChain {
			get => (int)NPC.ai[1];
			set => NPC.ai[1] = value;
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
                int bottomChain = NPC.whoAmI;
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
                int topChain = NPC.whoAmI;
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
            NPC.TargetClosest(true);
            if (LowerChain == 0 && UpperChain == 0)   
            {
                LowerChain = -1;
                int latestSegment = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y - NPC.height, ModContent.NPCType<Pokey_Body>(), 0, -1, NPC.whoAmI);
                UpperChain = latestSegment;
                for (int i = 2; i < segments; i++)
                {

                    Main.npc[latestSegment].ai[0] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y - (i * NPC.height), ModContent.NPCType<Pokey_Body>(), 0,-1, latestSegment);
                    latestSegment = (int)Main.npc[latestSegment].ai[0];
                }
            }
            if (!Head.active || Head.life <= 0)
            {
                NPC.life = 0;
				NPC.NPCLoot();
                return;
            }
            int queue = QueueFromBottom();
            if (queue == 0)
            {
                NPC.noGravity = false;
                //ai stuff for base goes here
                NPC.velocity.X += Math.Sign(Main.player[NPC.target].Center.X - NPC.Center.X) / 10f;
                NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);
                NPC.spriteDirection = -NPC.direction;
                Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
                CheckPlatform(Main.player[NPC.target]);
                CheckPit();
            }
            else
            {
                NPC.ai[2] += 0.15f;
                NPC.noGravity = true;
                NPC.aiStyle = -1;
                NPC.direction = Tail.direction;
                NPC.spriteDirection = -NPC.direction;
                NPC.position.X = Tail.position.X + ((float)Math.Sin(queue + NPC.ai[2]) * 3);
                float posToBe = Main.npc[LowerChain].position.Y - (NPC.height);
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
                NPC.frame.Y = 64;
            }
        }
        private void CheckPlatform(Player player)
        {
            bool onplatform = true;
            for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4)
            {
                Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
                if (!TileID.Sets.Platforms[tile.TileType])
                    onplatform = false;
            }
            if (onplatform && (NPC.Center.Y < player.position.Y - 20))
                NPC.noTileCollide = true;
            else
                NPC.noTileCollide = false;
        }
        private void CheckPit()
        {
            bool pit = true;
            for (int i = 1; i <= 4; i++)
            {
                Tile forwardtile = Framing.GetTileSafely(new Point((int)(NPC.Center.X / 16) + Math.Sign(NPC.velocity.X), (int)(NPC.Center.Y / 16) + i));
                if (WorldGen.SolidTile(forwardtile) || WorldGen.SolidTile2(forwardtile) || WorldGen.SolidTile3(forwardtile))
                    pit = false;
            }
            if (pit)
                NPC.velocity.X *= -0.5f;
        }
        private void GoTo(float pos) 
        {
            float lerpspeed = (Math.Abs(NPC.position.Y - pos) > (NPC.height / 2)) ? 0.45f : 0.15f;
            NPC.position.Y = MathHelper.Lerp(NPC.position.Y, pos, lerpspeed); 
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
            while (Main.npc[chain].ai[1] != NPC.whoAmI)
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
            while (Main.npc[chain].ai[0] != NPC.whoAmI)
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
            SoundEngine.PlaySound(SoundID.Dig, NPC.Center);

            Tail.velocity.X = hitDirection * 2;
        }

		public override bool CheckDead()
		{
			try {
				if (QueueFromBottom() != 0 || (Head.active && Head.life > 0))
					Delete();
			}
			catch (System.Exception) {
				throw new Exception("[Stactus] It's in delete number " + QueueFromBottom().ToString());
			}
			return true;
		}

		public override void OnKill()
        {
            if(Head.active && Head.life > 0) //no overlapping death sfx on head kill
                SoundEngine.PlaySound(SoundID.Dig, NPC.Center);

            switch (NPC.frame.Y)
            {
                case 0:
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pokey1_Gore1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pokey1_Gore2").Type, 1f);
                    break;
                case 32:
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pokey2_Gore1").Type, 1f);
                    break;
                case 64:
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PokeyHead_Gore1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PokeyHead_Gore2").Type, 1f);
                    break;
            }
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule headfulRule = new LeadingConditionRule(new DropRuleConditions.NPCConditional("", (npc) => npc.ModNPC is Pokey_Body body && body.Head.active && Head.life > 0));
			headfulRule.OnSuccess(ItemDropRule.Common(ItemID.Cactus, 1, 1, 3));

			LeadingConditionRule headlessRule = new LeadingConditionRule(new DropRuleConditions.NPCConditional("Drops only from the last segment", (npc) => npc.ModNPC is Pokey_Body body && Head.life <= 0));
			headlessRule.OnSuccess(ItemDropRule.Common(ItemID.CopperCoin, 1, 16, 25));
			headlessRule.OnSuccess(ItemDropRule.Common(ItemID.PinkPricklyPear));

			npcLoot.Add(headlessRule);
			npcLoot.Add(headfulRule);
		}
	}
}

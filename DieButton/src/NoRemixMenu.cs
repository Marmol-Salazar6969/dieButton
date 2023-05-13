using Menu.Remix.MixedUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RWCustom;

namespace DieButton
{
    public class OptionsMenu : OptionInterface
    {
        //First init all the values
        public static Configurable<KeyCode> Die;
        public static Configurable<bool> NormalDie { get; set; }
        public static Configurable<bool> StunDeath { get; set; }

        public static Configurable<bool> RandomDie { get; set; }
        public static Configurable<int> SpecificRandomDie { get; set; }


        public static Configurable<bool> CrazyDie { get; set; }
        public static Configurable<bool> RandomCrazyDie { get; set; }
        public static Configurable<int> SpecificCrazyDie { get; set; }


        public static Configurable<bool> AllTheWaysToDie { get; set; }

        public static Configurable<bool> OnlyOnDie { get; set; }

        /// <summary>
        /// DON NOT save this in the public build, it's just for testing things and see if it works
        /// </summary>
        //public static Configurable<bool> Test { get; set; }

        /// <summary>
        /// DON NOT save this in the public build, it's just for testing things and see if it works
        /// </summary>

        //all the check box or things that can be greyed out
        private OpCheckBox NormalDieBox;
        private OpCheckBox StunDeathBox;
        private OpCheckBox CrazyDieBox;
        private OpCheckBox AllTheWaysToDieBox;
        private OpCheckBox OnlyOnDieBox;
        private OpCheckBox RandomDieBox;
        private OpCheckBox RandomCrazyDieBox;
        

        //all the labels 
        private OpLabel NormalDieLabel;
        private OpLabel StunDeathLabel;
        private OpLabel CrazyDieLabel;
        private OpLabel AllTheWaysToDieLabel;
        private OpLabel OnlyOnDieLabel;
        private OpLabel RandomDieLabel;
        private OpLabel RandomCrazyDieLabel;

        /// <summary>
        /// TESTING DON'T BUILD THE LAST VERSION WITH THIS
        /// </summary>

        //private OpCheckBox TestBox;
        //private OpLabel TestLabel;

        /// <summary>
        /// TESTING DON'T BUILD THE LAST VERSION WITH THIS
        /// </summary>

        public OptionsMenu()
        {
            Die = config.Bind("Die", new KeyCode());

            NormalDie = config.Bind("NormalDie", false, new ConfigAcceptableList<bool>(false, true));
            StunDeath = config.Bind("StunDeath", false, new ConfigAcceptableList<bool>(false, true));

            RandomDie = config.Bind("RandomDie", false, new ConfigAcceptableList<bool>(false, true));
            SpecificRandomDie = config.Bind("SpecificRandomDie", 0, new ConfigAcceptableList<int>(0, 7));

            CrazyDie = config.Bind("CrazyDie", false, new ConfigAcceptableList<bool>(false, true));
            RandomCrazyDie = config.Bind("RandomCrazyDie", false, new ConfigAcceptableList<bool>(false, true));
            SpecificCrazyDie = config.Bind("SpecificCrazyDie", 0, new ConfigAcceptableList<int>(0, 7));

            AllTheWaysToDie = config.Bind("AllTheWaysToDie", false, new ConfigAcceptableList<bool>(false, true));

            OnlyOnDie = config.Bind("OnlyOnDie", false, new ConfigAcceptableList<bool>(false, true));


            /// <summary>
            /// DON NOT save this in the public build, it's just for testing things and see if it works
            /// </summary>
            //Test = config.Bind("Test", false, new ConfigAcceptableList<bool>(false, true));
            /// <summary>
            /// DON NOT save this in the public build, it's just for testing things and see if it works
            /// </summary>
        }
        public override void Update()
        {
            base.Update();
            Color colorOff = new Color(0.1451f, 0.1412f, 0.1529f);
            Color colorOn = new Color(0.6627f, 0.6431f, 0.698f);

            string caseValue = "";
            if (NormalDieBox.value == "true") caseValue = "NormalDieBox";
            else if (StunDeathBox.value == "true") caseValue = "StunDeathBox";
            else if (CrazyDieBox.value == "true") caseValue = "CrazyDieBox";
            else if (AllTheWaysToDieBox.value == "true") caseValue = "AllTheWaysToDieBox";
            else if (OnlyOnDieBox.value == "true") caseValue = "OnlyOnDieBox";
            else if (RandomDieBox.value == "true") caseValue = "RandomDieBox";
            else if (RandomCrazyDieBox.value == "true") caseValue = "RandomCrazyDieBox";
            //else if (TestBox.value == "true") caseValue = "TestBox";

            switch (caseValue)
            {
                case "NormalDieBox": //1
                    NormalDieBox.greyedOut = false;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOn;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "StunDeathBox": //2
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = false;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOn;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "CrazyDieBox": //3
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = false;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOn;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "AllTheWaysToDieBox": //4
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = false;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOn;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "OnlyOnDieBox": //5
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = false;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOn;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "RandomDieBox": //6
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = false;
                    RandomCrazyDieBox.greyedOut = true;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOn;
                    RandomCrazyDieLabel.color = colorOff;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                case "RandomCrazyDieBox": //7
                    NormalDieBox.greyedOut = true;
                    StunDeathBox.greyedOut = true;
                    CrazyDieBox.greyedOut = true;
                    AllTheWaysToDieBox.greyedOut = true;
                    OnlyOnDieBox.greyedOut = true;
                    RandomDieBox.greyedOut = true;
                    RandomCrazyDieBox.greyedOut = false;

                    NormalDieLabel.color = colorOff;
                    StunDeathLabel.color = colorOff;
                    CrazyDieLabel.color = colorOff;
                    AllTheWaysToDieLabel.color = colorOff;
                    OnlyOnDieLabel.color = colorOff;
                    RandomDieLabel.color = colorOff;
                    RandomCrazyDieLabel.color = colorOn;

                    //Don't leave this in the public version
                    //TestLabel.color = colorOff;
                    //TestBox.greyedOut = true;
                    break;

                //Don't leave this in the public version
                //case "TestBox": //8
                //    NormalDieBox.greyedOut = true;
                //    StunDeathBox.greyedOut = true;
                //    CrazyDieBox.greyedOut = true;
                //    AllTheWaysToDieBox.greyedOut = true;
                //    OnlyOnDieBox.greyedOut = true;
                //    RandomDieBox.greyedOut = true;
                //    RandomCrazyDieBox.greyedOut = true;

                //    NormalDieLabel.color = colorOff;
                //    StunDeathLabel.color = colorOff;
                //    CrazyDieLabel.color = colorOff;
                //    AllTheWaysToDieLabel.color = colorOff;
                //    OnlyOnDieLabel.color = colorOff;
                //    RandomDieLabel.color = colorOff;
                //    RandomCrazyDieLabel.color = colorOff;

                //    TestLabel.color = colorOn;
                //    TestBox.greyedOut = false;
                //    break;

                default: //9
                    NormalDieBox.greyedOut = false;
                    StunDeathBox.greyedOut = false;
                    CrazyDieBox.greyedOut = false;
                    AllTheWaysToDieBox.greyedOut = false;
                    OnlyOnDieBox.greyedOut = false;
                    RandomDieBox.greyedOut = false;
                    RandomCrazyDieBox.greyedOut = false;

                    NormalDieLabel.color = colorOn;
                    StunDeathLabel.color = colorOn;
                    CrazyDieLabel.color = colorOn;
                    AllTheWaysToDieLabel.color = colorOn;
                    OnlyOnDieLabel.color = colorOn;
                    RandomDieLabel.color = colorOn;
                    RandomCrazyDieLabel.color = colorOn;

                    //Don't leave this
                    //TestLabel.color = colorOn;
                    //TestBox.greyedOut = false;
                    break;
            }
        }

        public override void Initialize()
        {
            var opTab1 = new OpTab(this, Translate("Options"));
            var opTab2 = new OpTab(this, Translate("Testing"));
            Tabs = new[] { opTab1/*Testing tab, opTab2*/ };

            OpContainer tab1Container = new OpContainer(new Vector2(0, 0));
            opTab1.AddItems(tab1Container);

            /// <summary>
            /// DON NOT save this in the public build, it's just for testing things and see if it works
            /// </summary>

            //for (int i = 0; i <= 600; i += 10)
            //{
            //    Color c;
            //    c = Color.grey;
            //    if (i % 50 == 0) { c = Color.yellow; }
            //    if (i % 100 == 0) { c = Color.red; }
            //    FSprite lineSprite = new FSprite("pixel");
            //    lineSprite.color = c;
            //    lineSprite.alpha = 0.2f;
            //    lineSprite.SetAnchor(new Vector2(0.5f, 0f));
            //    Vector2 a = new Vector2(i, 0);
            //    lineSprite.SetPosition(a);
            //    Vector2 b = new Vector2(i, 600);
            //    float rot = Custom.VecToDeg(Custom.DirVec(a, b));
            //    lineSprite.rotation = rot;
            //    lineSprite.scaleX = 2f;
            //    lineSprite.scaleY = Custom.Dist(a, b);
            //    tab1Container.container.AddChild(lineSprite);
            //    a = new Vector2(0, i);
            //    b = new Vector2(600, i);
            //    lineSprite = new FSprite("pixel");
            //    lineSprite.color = c;
            //    lineSprite.alpha = 0.2f;
            //    lineSprite.SetAnchor(new Vector2(0.5f, 0f));
            //    lineSprite.SetPosition(a);
            //    rot = Custom.VecToDeg(Custom.DirVec(a, b));
            //    lineSprite.rotation = rot;
            //    lineSprite.scaleX = 2f;
            //    lineSprite.scaleY = Custom.Dist(a, b);
            //    tab1Container.container.AddChild(lineSprite);
            //}

            /// <summary>
            /// DON NOT save this in the public build, it's just for testing things and see if it works
            /// </summary>

            UIelement[] UIArrayElements1 = new UIelement[]
            {
                //Keybind
                new OpKeyBinder(Die, new Vector2(10,540), new Vector2(0,0)) { description = Translate("Set die button, pressing this in-game will kill you") },
                new OpLabel(45f, 540f, Translate("Keybind for dying")),

                //Bools
                NormalDieBox = new OpCheckBox(NormalDie, 10, 500) { description = Translate("Enables only the natural die of the Slugcat") },
                NormalDieLabel = new OpLabel(45f, 500f, Translate("Normal Die")),

                StunDeathBox = new OpCheckBox(StunDeath, 10, 460) { description = Translate("Enables the death on stun feature") },
                StunDeathLabel = new OpLabel(45f, 460f, Translate("Stun Die")),

                CrazyDieBox = new OpCheckBox(CrazyDie, 10, 420) { description = Translate("Enables a crazy way to die") },
                CrazyDieLabel = new OpLabel(45f, 420f, Translate("Crazy Die")),

                AllTheWaysToDieBox = new OpCheckBox(AllTheWaysToDie, 10, 380) { description = Translate("Enables die of every day doesn't matters the category") },
                AllTheWaysToDieLabel = new OpLabel(45f, 380f, Translate("All The Ways To Die")),

                OnlyOnDieBox = new OpCheckBox(OnlyOnDie, 10, 340) { description = Translate("Enables the random die features ONLY ON DIE") },
                OnlyOnDieLabel = new OpLabel(45f, 340f, Translate("Only On Die")),

                RandomDieBox = new OpCheckBox(RandomDie, 10, 300) { description = Translate("Enables the random die features ON KEY") },
                RandomDieLabel = new OpLabel(45f, 300f, Translate("Random Die")),

                RandomCrazyDieBox = new OpCheckBox(RandomCrazyDie, 10, 260) { description = Translate("Enables all the random Crazy ways to die!") },
                RandomCrazyDieLabel = new OpLabel(45f, 260f, Translate("Random Crazy Die")),
            };
            opTab1.AddItems(UIArrayElements1);

            /// <summary>
            /// DON NOT save this in the public build, it's just for testing things and see if it works
            /// </summary>

            //OpContainer containerTab2 = new OpContainer(new Vector2(0, 0));
            //opTab2.AddItems(containerTab2);
            
            //UIArrayElements1 = new UIelement[]
            //{
            //    TestBox = new OpCheckBox(Test, 10, 540) { description = Translate("The text box, I hope you are debugging") },
            //    TestLabel = new OpLabel(45f, 540f, Translate("Testing mode")),
            //};
            //opTab2.AddItems(UIArrayElements1);
        }

        /// <summary>
        /// DON NOT save this in the public build, it's just for testing things and see if it works
        /// </summary>
    }
}

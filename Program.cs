using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SynCatGenerator
{
    public class SynCat
    {
        public List<string> sg_nouns = new List<string>() { "block", "box", "mug", "cup", "knife", "plate", "one" };
        public List<string> pl_nouns = new List<string>() { "blocks", "boxes", "mugs", "cups", "knives", "plates", "ones" };
        public List<string> sg_determiners = new List<string>() { "this", "that", "the" };
        public List<string> pl_determiners = new List<string>() { "these", "those", "the" };
        public List<string> adjectives = new List<string>(){ "yellow", "red", "blue", "purple", "green", "orange", "white",
        "gray", "black", "pink", "brown" };
        public List<string> shift = new List<string>() { "nevermind", "wait" };
        public List<string> trans_no_goal = new List<string>() { "pick up", "lift", "grab", "grasp", "take" };
        public List<string> trans_goal = new List<string>() { "move", "put", "push", "pull", "slide", "place" };
        public List<string> prepositions = new List<string>(){ "on the left of", "on the right of", "to the left of",
        "to the right of", "left of", "right of", "above", "below", "behind", "in", "on",
        "beside", "before", "around",  "on top of", "in front of", "in back of", "on the front of",
        "on the back of"};
        //making lists static so multiple searches are quicker 
        public List<string> nps = new List<string>();
        public List<string> pps = new List<string>();
        public List<string> vps = new List<string>();
        public List<string> partial_vps = new List<string>();
        public List<string> sg_partial_nps = new List<string>();
        public List<string> pl_partial_nps = new List<string>();
        //Transform getUserIntentObjectObj;
        //Transform getUserIntentLocationObj;

        public SynCat()
        {
	    /*
            this.nps = GetNPs();
            this.pps = GetPPs();
            this.vps = GetVPs();
            this.partial_vps = VPsNeedGoal();
            this.sg_partial_nps = SgPartialNPs();
            this.pl_partial_nps = PlPartialNPs();
	    */
        }

        public List<string> GetPPs()
        {
            List<string> PPs = new List<string>();
            List<string> NPs = GetNPs();
            //Only went down a depth of one since
            //Diana can't seem to handle
            //'the block to the left of the yellow block' 
            foreach (string p in prepositions)
            {
                foreach (string np in NPs)
                {
                    string[] words = np.Split(' ');
                    string first = words[0];
                    if (sg_determiners.Contains(first) || pl_determiners.Contains(first))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(p).Append(" ").Append(np);
                        PPs.Add(builder.ToString().Trim());
                    }
                }
            }
            return PPs;
        }

        public List<string> GetNPs()
        {
            List<string> sg_partials = SgPartialNPs();
            List<string> pl_partials = PlPartialNPs();
            List<string> sg_full = new List<string>();
            List<string> pl_full = new List<string>();
            List<string> NPs = new List<string>();


            foreach (string sg_det in sg_determiners)
            {
                foreach (string nn in sg_partials)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(sg_det).Append(" ").Append(nn);
                    sg_full.Add(builder.ToString().Trim());
                }
            }

            foreach (string pl_det in pl_determiners)
            {
                foreach (string nn in pl_partials)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(pl_det).Append(" ").Append(nn);
                    pl_full.Add(builder.ToString().Trim());
                }
            }

            NPs.AddRange(sg_partials);
            NPs.AddRange(pl_partials);
            NPs.AddRange(sg_full);
            NPs.AddRange(pl_full);
            return NPs;
        }

        public List<string> GetVPs()
        {
            List<string> PPs = GetPPs();
            List<string> NPs = GetNPs();
            List<string> VPs = new List<string>();
            List<string> VP_trans_no_goal = new List<string>();
            List<string> VP_trans_goal = new List<string>();
            List<string> shift_theme = new List<string>();
            List<string> shift_goal = new List<string>();

            foreach (string vtng in trans_no_goal)
            {
                foreach (string np in NPs)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(vtng).Append(" ").Append(np);
                    VP_trans_no_goal.Add(builder.ToString().Trim());
                }
            }

            foreach (string vtg in trans_goal)
            {
                foreach (string np in NPs)
                {
                    foreach (string pp in PPs)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(vtg).Append(" ").Append(np).Append(" ").Append(pp);
                        VP_trans_goal.Add(builder.ToString().Trim());
                    }
                }
            }

            foreach (string vs in shift)
            {
                foreach (string np in NPs)
                {
                    // if we need to allow for things like
                    // 'wait, red one' then we can remove the guard
                    string[] words = np.Split(' ');
                    string first = words[0];
                    if (sg_determiners.Contains(first) || pl_determiners.Contains(first))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(vs).Append(" ").Append(np);
                        shift_theme.Add(builder.ToString().Trim());
                    }
                }

                foreach (string pp in PPs)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(vs).Append(" ").Append(pp);
                    shift_goal.Add(builder.ToString().Trim());
                }
            }
            VPs.AddRange(shift);
            VPs.AddRange(shift_theme);
            VPs.AddRange(shift_goal);
            VPs.AddRange(VP_trans_no_goal);
            VPs.AddRange(VP_trans_goal);
            return VPs;
        }

        public List<string> VPsNeedGoal()
        {
            List<string> NPs = GetNPs();
            List<string> VP_trans_goal = new List<string>();

            foreach (string vtg in trans_goal)
            {
                foreach (string np in NPs)
                {
                  StringBuilder builder = new StringBuilder();
                  builder.Append(vtg).Append(" ").Append(np);
                  VP_trans_goal.Add(builder.ToString().Trim());
                }
            }

            return VP_trans_goal;
        }

        public List<string> SgPartialNPs()
        {
            List<string> sg_adj_no_det = new List<string>();
            List<string> NPs = new List<string>();

            foreach (string adj in adjectives)
            {
                foreach (string nn in sg_nouns)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(adj).Append(" ").Append(nn);
                    sg_adj_no_det.Add(builder.ToString().Trim());
                }
            }

            NPs.AddRange(sg_nouns);
            NPs.AddRange(sg_adj_no_det);
            return NPs;
        }

        public List<string> PlPartialNPs()
        {
            List<string> pl_adj_no_det = new List<string>();
            List<string> NPs = new List<string>();

            foreach (string adj in adjectives)
            {
                foreach (string nn in pl_nouns)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(adj).Append(" ").Append(nn);
                    pl_adj_no_det.Add(builder.ToString().Trim());
                }
            }

            NPs.AddRange(pl_adj_no_det);
            NPs.AddRange(pl_nouns);
            return NPs;
        }
        public List<string> Predict()
        {
            /*
            // child of GoogleSR object that stores "user:intent:object"
            //getUserIntentObjectObj = transform.Find("GetUserIntentObject");

            // child of GoogleSR object that stores "user:intent:location"
            //getUserIntentLocationObj = transform.Find("GetUserIntentLocation");

            // get the retrieved value of "user:intent:object" by:
            var userIntentObject = (string)getUserIntentObjectObj.GetComponent("GetBlackboardEntry").GetType().GetField("stringValue").
                GetValue(getUserIntentObjectObj.GetComponent("GetBlackboardEntry"));
            Debug.Log(string.Format("userIntentObject = {0}", userIntentObject));
            // if userIntentObject is not null, then there's an object on the blackboard

            // get the retrieved value of "user:intent:location" by:
            var userIntentLocation = (Vector3)getUserIntentLocationObj.GetComponent("GetBlackboardEntry").GetType().GetField("vector3Value").
                GetValue(getUserIntentLocationObj.GetComponent("GetBlackboardEntry"));
            Debug.Log(string.Format("userIntentLocation = {0}", userIntentLocation));
            // if userIntentLocation is not (0,0,0), then there's an location on the blackboard
            // initializing otherwise VS complains in the conditionals
            */
	    
            string syn_item = "";
    	    Vector3 loc = Vector3.zero;
            bool s_mod = false;
            bool l_mod = false;
	        List<string> Predictions = new List<string>();
	        for (int i = 0; i < args.Length; i++)
	        {
		        if (args[i] is String)
		        {
		            syn_item = (string)args[i];
                    s_mod = true;
		        }
		        if (args[i] is Vector3)
		        {
		            loc = (Vector3)args[i];
                    l_mod = true;
		        }
	        }
            
            

            // would rather use a switch statement but there 
            // did not seem to be a way that was clearer
            // or more efficient than the series of if-statements
            // below
            if (!s_mod)
            {
                return this.nps;
            }

            // !l_mod reasoning:  if no location has been stored we need to bias for a PP
            // likewise if we've just seen a full or partial NP 
            // (since if there's no goal the speaker won't say anything)
            if (!l_mod || partial_vps.Any(m => m == syn_item) ||
                nps.Any(m => m == syn_item) || 
                sg_partial_nps.Any(m => m == syn_item) || 
                pl_partial_nps.Any(m => m == syn_item))
            {
                return this.pps;
            }

            // for shift verbs we want to bias towards a new object
            // or location
            if (shift.Any(m => m == syn_item))
            {
                Predictions.AddRange(this.nps);
                Predictions.AddRange(this.pps);
                return Predictions;
            }

            // all V-heads we're likely to deal with immediately subcat for an NP, 
            // likewise for all P-heads
            if (trans_goal.Any(m => m == syn_item) || trans_no_goal.Any(m => m == syn_item) || prepositions.Any(m => m == syn_item))
            {
                return this.nps;
            }

            if (sg_determiners.Any(m => m == syn_item))
            {
                return sg_partial_nps;
            }

            if (pl_determiners.Any(m => m == syn_item))
            {
                return pl_partial_nps;
            }

            //default
            
            List<string> nps = GetNPs();
            return nps;
        }
        public static void Main(string[] args)
        {
            //main is here just to check if
            //this can compile by itself
            
            SynCat r = new SynCat();
            foreach (string line in r.Predict())
            {
            Console.WriteLine(line);
            }
            
        }
    }
}

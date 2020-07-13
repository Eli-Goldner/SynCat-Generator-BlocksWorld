using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SynCatGenerator
{
    public class SynCat
    {
        public HashSet<string> sg_nouns = new HashSet<string>() { "block", "box", "mug", "cup", "knife", "plate", "one" };
        public HashSet<string> pl_nouns = new HashSet<string>() { "blocks", "boxes", "mugs", "cups", "knives", "plates", "ones" };
        public HashSet<string> sg_determiners = new HashSet<string>() { "this", "that", "the" };
        public HashSet<string> pl_determiners = new HashSet<string>() { "these", "those", "the" };
        public HashSet<string> adjectives = new HashSet<string>(){ "yellow", "red", "blue", "purple", "green", "orange", "white",
        "gray", "black", "pink", "brown" };
        public HashSet<string> shift = new HashSet<string>() { "nevermind", "wait" };
        public HashSet<string> trans_no_goal = new HashSet<string>() { "pick up", "lift", "grab", "grasp", "take" };
        public HashSet<string> trans_goal = new HashSet<string>() { "move", "put", "push", "pull", "slide", "place" };
        public HashSet<string> prepositions = new HashSet<string>(){ "on the left of", "on the right of", "to the left of",
        "to the right of", "left of", "right of", "above", "below", "behind", "in", "on",
        "beside", "before", "around",  "on top of", "in front of", "in back of", "on the front of",
        "on the back of"};
        //making lists static so multiple searches are quicker 
        public HashSet<string> nps = new HashSet<string>();
        public HashSet<string> pps = new HashSet<string>();
        public HashSet<string> vps = new HashSet<string>();
        public HashSet<string> partial_vps = new HashSet<string>();
        public HashSet<string> sg_partial_nps = new HashSet<string>();
        public HashSet<string> pl_partial_nps = new HashSet<string>();
        //Transform getUserIntentObjectObj;
        //Transform getUserIntentLocationObj;

        public SynCat()
        {
	    this.sg_partial_nps = SgPartialNPs();
            this.pl_partial_nps = PlPartialNPs();
            this.nps = GetNPs();
            this.pps = GetPPs();
            this.partial_vps = VPsNeedGoal();
        }

        public HashSet<string> GetPPs()
        {
	    HashSet<string> NPs = GetNPs();
            HashSet<string> PPs = new HashSet<string>();
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

        public HashSet<string> GetNPs()
        {
	    HashSet<string> sg_partials = SgPartialNPs();
	    HashSet<string> pl_partials = PlPartialNPs();
	    HashSet<string> sg_full = new HashSet<string>();
            HashSet<string> pl_full = new HashSet<string>();
            HashSet<string> NPs = new HashSet<string>();


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

            NPs.UnionWith(sg_partials);
            NPs.UnionWith(pl_partials);
            NPs.UnionWith(sg_full);
            NPs.UnionWith(pl_full);
            return NPs;
        }

	/*
        public HashSet<string> GetVPs()
        {
            HashSet<string> PPs = GetPPs();
            HashSet<string> NPs = GetNPs();
            HashSet<string> VPs = new HashSet<string>();
            HashSet<string> VP_trans_no_goal = new HashSet<string>();
            HashSet<string> VP_trans_goal = new HashSet<string>();
            HashSet<string> shift_theme = new HashSet<string>();
            HashSet<string> shift_goal = new HashSet<string>();

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
            VPs.UnionWith(shift);
            VPs.UnionWith(shift_theme);
            VPs.UnionWith(shift_goal);
            VPs.UnionWith(VP_trans_no_goal);
            VPs.UnionWith(VP_trans_goal);
            return VPs;
        }
	*/
	
        public HashSet<string> VPsNeedGoal()
        {
	    HashSet<string> NPs = GetNPs();
            HashSet<string> VP_trans_goal = new HashSet<string>();

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

        public HashSet<string> SgPartialNPs()
        {
            HashSet<string> sg_adj_no_det = new HashSet<string>();
            HashSet<string> NPs = new HashSet<string>();

            foreach (string adj in adjectives)
            {
                foreach (string nn in sg_nouns)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(adj).Append(" ").Append(nn);
                    sg_adj_no_det.Add(builder.ToString().Trim());
                }
            }

            NPs.UnionWith(sg_nouns);
            NPs.UnionWith(sg_adj_no_det);
            return NPs;
        }

        public HashSet<string> PlPartialNPs()
        {
            HashSet<string> pl_adj_no_det = new HashSet<string>();
            HashSet<string> NPs = new HashSet<string>();

            foreach (string adj in adjectives)
            {
                foreach (string nn in pl_nouns)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(adj).Append(" ").Append(nn);
                    pl_adj_no_det.Add(builder.ToString().Trim());
                }
            }

            NPs.UnionWith(pl_adj_no_det);
            NPs.UnionWith(pl_nouns);
            return NPs;
        }

	public List<string> Predict(string syn_item)
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
	    
            
    	    //Vector3 loc = Vector3.zero;
	    List<string> Predictions = new List<string>();
            // would rather use a switch statement but there 
            // did not seem to be a way that was clearer
            // or more efficient than the series of if-statements
            // below
	    
            if (String.IsNullOrEmpty(syn_item))
            {
                Predictions.AddRange(this.nps.ToList());
		Predictions.AddRange(this.partial_vps.ToList());
		return Predictions;
	    }

	    // for shift verbs we want to bias towards a new object
            // or location
            if (this.shift.Contains(syn_item))
            {
                Predictions.AddRange(this.nps.ToList());
                Predictions.AddRange(this.pps.ToList());
                return Predictions;
            }
	    
            if (this.sg_determiners.Contains(syn_item))
            {
                return sg_partial_nps.ToList();
            }

            if (this.pl_determiners.Contains(syn_item))
            {
                return pl_partial_nps.ToList();
            }

	    // all V-heads we're likely to deal with immediately subcat for an NP, 
            // likewise for all P-heads
            if (this.trans_goal.Contains(syn_item) ||
		this.trans_no_goal.Contains(syn_item) ||
		this.prepositions.Contains(syn_item))
            {
                return this.nps.ToList();
            }
	    
            // !l_mod reasoning:  if no location has been stored we need to bias for a PP
            // likewise if we've just seen a full or partial NP 
            // (since if there's no goal the speaker won't say anything)
            if (this.partial_vps.Contains(syn_item) ||
                this.nps.Contains(syn_item) || 
                this.sg_partial_nps.Contains(syn_item) || 
                this.pl_partial_nps.Contains(syn_item))
            {
                return this.pps.ToList();
            }

            //default
            return this.nps.ToList();
        }

	public static void Main(string[] args)
        {
            //main is here just to check if
            //this can compile by itself
	    Random rnd = new Random();
	    SynCat r = new SynCat();
	    int pv_idx = rnd.Next(r.partial_vps.Count);
	    int pl_idx = rnd.Next(r.pl_partial_nps.Count);
	    int tng_idx = rnd.Next(r.trans_no_goal.Count);
	    int sgdet_idx = rnd.Next(r.sg_determiners.Count);
	    int nps_idx = rnd.Next(r.nps.Count);
	    List<string> tests = new List<string> {r.partial_vps.ToList()[pv_idx],
		r.pl_partial_nps.ToList()[pl_idx],
		r.trans_no_goal.ToList()[tng_idx],
		r.sg_determiners.ToList()[sgdet_idx],
		r.nps.ToList()[nps_idx],
		"____NOT_A_SYNCAT____",
		""};
	    List<string> shuffled = tests.OrderBy(item => rnd.Next()); 
	    foreach (string elem in shuffled)
	    {
		//Console.WriteLine(elem);
		r.Predict(elem);
	    }
	}
    }
}

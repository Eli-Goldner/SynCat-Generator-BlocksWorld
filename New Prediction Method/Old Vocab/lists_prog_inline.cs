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
	    this.sg_partial_nps = SgPartialNPs();
            this.pl_partial_nps = PlPartialNPs();
            this.nps = GetNPs(this.sg_partial_nps, this.pl_partial_nps);
            this.pps = GetPPs(this.nps);
            this.partial_vps = VPsNeedGoal(this.nps);
        }

        public List<string> GetPPs(List<string> NPs)
        {
            List<string> PPs = new List<string>();
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

        public List<string> GetNPs(List<string> sg_partials, List<string> pl_partials)
        {
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

	/*
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
	*/
	
        public List<string> VPsNeedGoal(List<string> NPs)
        {
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
                Predictions.AddRange(this.nps);
		Predictions.AddRange(this.partial_vps);
		return Predictions;
	    }

	    // for shift verbs we want to bias towards a new object
            // or location
            if (this.shift.Contains(syn_item))
            {
                Predictions.AddRange(this.nps);
                Predictions.AddRange(this.pps);
                return Predictions;
            }
	    
            if (this.sg_determiners.Contains(syn_item))
            {
                return sg_partial_nps;
            }

            if (this.pl_determiners.Contains(syn_item))
            {
                return pl_partial_nps;
            }

	    // all V-heads we're likely to deal with immediately subcat for an NP, 
            // likewise for all P-heads
            if (this.trans_goal.Contains(syn_item) ||
		this.trans_no_goal.Contains(syn_item) ||
		this.prepositions.Contains(syn_item))
            {
                return this.nps;
            }
	    
            // !l_mod reasoning:  if no location has been stored we need to bias for a PP
            // likewise if we've just seen a full or partial NP 
            // (since if there's no goal the speaker won't say anything)
            if (this.partial_vps.Contains(syn_item) ||
                this.nps.Contains(syn_item) || 
                this.sg_partial_nps.Contains(syn_item) || 
                this.pl_partial_nps.Contains(syn_item))
            {
                return this.pps;
            }

            //default
            return this.nps;
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
	    var shuffled = tests.OrderBy(item => rnd.Next()); 
	    foreach (string elem in shuffled)
	    {
		//Console.WriteLine(elem);
		r.Predict(elem);
	    }
	}

    }
}

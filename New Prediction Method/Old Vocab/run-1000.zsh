for run in {1..1000}; do (time mono $1) >> $2 2>&1; done

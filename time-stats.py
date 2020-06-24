import sys
from statistics import mean, median

def main():
    f_name = sys.argv[1]
    f = open(f_name, 'r')
    times = [float(line.split()[-2]) for line in f]

    print("minimum: {0}, maximum: {1}, average: {2}, median: {3}".format(min(times),
                                                                         max(times),
                                                                         '{:.3f}'.format(mean(times)),
                                                                         '{:.3f}'.format(median(times))))

if __name__ == "__main__":
    main()

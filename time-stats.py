import sys

f_name = sys.argv[1]
f = open(f_name, 'r')

times = [float(line.split()[-2]) for line in f]

def avg(ls):
    return format(sum(ls) / len(ls), '.2f')

print("minimum: {0}, maximum: {1}, average: {2}".format(min(times), max(times), avg(times)))

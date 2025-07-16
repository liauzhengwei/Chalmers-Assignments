# Quickstart guide

This guide will help you get started with the Minerva cluster. The guide covers essentially two things:

- How to create local environments
- How to run jobs through Slurm (using your locally installed packages)

## Local environments
The main system environment is intentionally kept very sparse and contains little to no applications of interest. This is to reduce clutter and maintenance of the global systems. Instead, you are expected to install your own packages **yourself** under your home directory.

Since most work on the cluster is done using the Python ecosystem, the recommended ways are to either use Python virtual environments (venvs) or Anaconda (Miniforge). The latter is preferred, but the former is a simpler and a more lightweight solution. We will next cover both of these options.

### Virtual environments
Python Virtual environments are a standardized solution that allows for the creation of separated environments where Python packages can be installed so that they do not interfere with each other. To find out more about Venvs, please have a look at the [Python venv module documentation](https://docs.python.org/3/library/venv.html).

In a nutshell, a venv is simply a directory that contains the Python ecosystem that you want to use. It is enabled by executing an activation script.

#### Creating a venv
Suppose you want to create a venv called `myenv` under your home directory. This is done by executing the following command:

```
$ python3 -m venv create ~/myenv
```

This creates a new directory called `~/myenv` that contains your environment. That's it! If later on you want to remove it, you simply delete this directory. However, in order to use it, you must activate it.

#### Activating a venv
A venv is activated by **sourcing** an activation script that is stored under `~/myenv/bin/activate`. This is simply done by the following script:

```
$ source ~/myenv/bin/activate
```

If successfull, your **prompt** should now indicate the active venv by `(myenv)`. All future Python commands are now executed with respect to this environment (particularly the installation of packages).

If you want to deactivate the environment, simply execute the command:

```
$ deactivate
```

Now `(myenv)` should disappear from the prompt.

**Nice to know:** What `source` does is that it executes the script **in the current shell**. This means that the variable definitions that are executed by the activation script remain active in the shell even after the execution of the script. So the script essentially injects new environment variables into the executing shell.

#### Installing packages

New packages can be installed using `pip`. For example, to install NumPy, make sure the environment is active and issue the following command:

```
$ python3 -m pip install numpy
```

#### Help! My environment is all messed up!

What's nice about the venvs is that since they are isolated, if you mess up something, you can simply nuke the whole environment. So just delete it all (make sure you've deactivated the environment first):

```
$ rm -vr ~/myenv
```

Then you can start from scratch.

### Anaconda (Miniforge)

[Anaconda](https://www.anaconda.com/) is a Python distribution that comes with an extensive `conda` package management system that allows for the easy installation of most packages people need in well-isolated environments.

#### Installing Anaconda (Miniforge)

We recommend using the [Miniforge](https://conda-forge.org/download/) distribution because of its small size and integration with [Conda Forge](https://conda-forge.org/).

Go to the [Miniforge website](https://conda-forge.org/download/) and select the link for [Linux x86_64 (amd64)](https://github.com/conda-forge/miniforge/releases/latest/download/Miniforge3-Linux-x86_64.sh). You can dowload the package directly to your home directory with the following command:

```
$ wget https://github.com/conda-forge/miniforge/releases/latest/download/Miniforge3-Linux-x86_64.sh
```

To install Miniforge, simply execute the file you just downloaded using `bash`:

```
$ bash Miniforge3-Linux-x86_64.sh
```

The installer will ask you to say yes to the license, the installation path, and whether to add automatic initialization of Miniforge in your shell profile.

By default, Miniforge is installed under your home directory `~/miniforge3/`, and we shall assume that this is the case for the remainder of this guide.

We recommend that you do not initialize Miniforge automatically, although this has little impact.

To delete Miniforge, you simply need to delete the directory `~/miniforge3/`. Also, if you chose to update your profile, you may want to reverse the changes.

If you prefer the official Anaconda installation, the process is very similar, except you download the Anaconda installer from [anaconda.com](https://www.anaconda.com) and the default installation location becomes `~/anaconda3/`.


#### Activating Anaconda (Miniforge)

Anaconda is essentially a very fancy venv that comes with a lot of bells and whistles, most importantly the conda (and mamba) package managers.

To activate Miniforge, just issue the following command:

```
$ source ~/miniforge3/bin/activate
```

You should be greeted with a `(base)` in the prompt of your shell, indicating that the currently active Anaconda environment is the `base` environment.

You can deactivate Anaconda with

```
$ conda deactivate
```

#### Creating and managing environments

It is recommended to keep as few packages as possible in the base environment because this can adversely affect package installation (`conda` is notoriously slow at resolving dependencies and the packages may create conflicts). Instead, it is recommended that isolated environments are created for each use case.

To create an environment called `myenv`, issue the following command:

```
$ conda create -n myenv
```

You can then activate the environment with

```
$ conda activate myenv
```

You can also state which packages and which versions should be installed upon creating an environment. This is especially useful if you need, for example, a particular version of Python:
```
$ conda create -n myenv python==3.13
```

You can get a list of all environments with

```
$ conda env list
```

To remove an environment with all packages:

```
$ conda remove -n myenv --all
```

#### Managing packages

Packages can be installed, updated, and removed with conda.

To install a package, activate the environment you want to install the packages in and issue the command `conda install packagename`. For example, to install NumPy:

```
$ conda install numpy
```

You can also specify constraints on the version numbers, for example, to enforce a particular version, or to require an older version, for example,

```
$ conda install numpy==2.2.3
```

You can update packages with `conda update`:

```
$ conda update numpy
```

And you can remove packages with `conda remove`:

```
$ conda remove numpy
```

## SLURM

### Introduction
[SLURM](https://en.wikipedia.org/wiki/Slurm_Workload_Manager) is a queuing system that enables running jobs on computer clusters. The idea is that, when there are more users than there are computers, users need to wait for sufficient resources to be available for their jobs, which are then executed by the workload manager. That is, the user schedules a job to be run, specifies what resources their job needs (e.g., the number of CPUs and GPUs, the amount of memory etc.), submit their job into the queue, and the job is executed at some point in the future. The results are then written on a disk. The job runs as if the user was executing it, but the user may not know in advance when and on which computer it is executed.

### Showing your information

All users need to be added into **accounting** which records the users actions (e.g., how much resources they've used). You can view your user account entry with 

```
$ sacctmgr show user withassoc where name=CID
```

where you replace CID with your own CID.

**Note.** If the result is **empty** this suggests you have not been added to the accounting and subsequent commands may fail. Please send email to karppa(at)chalmers.se about the issue.

### Showing cluster information

The cluster is organized into **partitions**. Partitions are (potentially overlapping) subsets of the resources (computers, CPUs, memory, GPUs) available on the cluster. Each partition is associated with a **queue**. You can view the cluster status with

```
$ sinfo
```

There are two relevant queues in the system: `short` and `long`. For most course work, you should use the `short` queue, which is also the default. The short queue limits jobs to last at most 30 minutes, after which they are killed. This is to prevent inadvertently hogging system resources with hanging jobs.

### Enequing jobs

In order to run a job, you need to create a **sbatch script** and then run it with `sbatch`. The script is an ordinary shell script that additionally contains `#SBATCH` directives at the top, special commands that tell Slurm what kind of resources to allocate for the job and what other needs there exist.

For example, suppose we store the following into file `job.sh`:

```
#!/bin/bash

#SBATCH -c 1
#SBATCH --mem=1G
#SBATCH -t 0:01:00

echo `whoami` is running on `hostname`
```

The job executes the echo command and sets the following paramteters: 1 CPU and 1 GiB memory requested, maximum time at 1 minute.

**Note.** Requesting fewer resources means your job will get priority sooner, so only ask for resources you need.

We can then eneque the job by issuing

```
$ sbatch job.sh
```

We then get acknowledged with the job id, for example:

```
Submitted batch job 2961
```

This means that our job has id 2961. By default, the results (stdout and stderr output) are stored in a file called `slurm-ID.out`. So in this case:

```
$ cat slurm-2961.out
karppa is running on callisto
```

### Checking the status of jobs

You can see the queue status with `squeue`. By default, **all** jobs are shown, however, the parameter `-u` enables you to see only jobs by a particular user. A useful idiom: `squeue -u $USER` shows your own jobs.

For example,

```
$ squeue -u $USER
             JOBID PARTITION     NAME     USER ST       TIME  NODES NODELIST(REASON)
              2962     short test.sba   karppa  R       0:04      1 callisto
```
shows that my only running job has been executing on `callisto` for 4 seconds.

### Canceling a job

If you want to cancel a job, you can issue `scancel id` where `id` would ber replaced by your job id.

**Idiom.** To cancel all your own jobs, issue `scancel -u $USER`.

### SBATCH options

There are very many options to SBATCH, here are some of the most useful:

- The number of CPU cores is specified by `-c n` or `--cpus-per-task=n` where `n` is the number of cores requested.
- The amount of memory can be specified by `--mem=amount` where `amount` is by default in megabytes, or with suffixes in other units. For example, `1G` would correspond to 1 gigabyte. Alternatively, we can request memory by the CPU or by the GPU with `--mem-per-cpu=amount` or `--mem-per-gpu=amount`. For example, `--mem-per-cpu=1G` would request 1 GiB of memory per CPU.
- Time limit for the job can be specified with `-t time` or `--time=time`. The argument `time` has format `hh:mm:ss`, so `-t 0:20:00` would request 20 minutes of wall clock time for the job.
- The partition can be requested with `-p partition` or `--partition=partition`. For example, `-p short` would request the job to be run in the `short` partition.
- By default, both stdout and stderr are redirected to a file called `slurm-id.out`. If you want to redirect them into a different file, use `-o file` or `--output=file` to redirect stdout and `-e file` and `--error=file` to redirect stderr. **Tip.** Including `%j` is replaced with the job id. **Example.** `-e job-%j.err` redirects stderr to `job-id.err` where `id` is the job id.
- Job name can be specified with `-J name` or `--job-name=name`. This shows up in `squeue` and makes it easier to identify the job.
- GPUs can be requested with `--gres=gpu:n` where `n` is the number of GPUs requested. Alternatively, we can specify the type of the GPU with `--gres=gpu:type:n`. The allowed values for `type` are `L40s` (8 in total available, at most 4 per node) and `L4` (32 in total available, at most 8 per node).
- To request that the job is run on a specified node, use `-w node` or `--nodelist=node`. For example, `-w callisto` requests that the job be run on `callisto`.


#### Example

The following script would 

```
#!/bin/bash
#SBATCH -c 4
#SBATCH --mem-per-cpu=1G
#SBATCH -J FancyJob
#SBATCH -e fancy-%j.err
#SBATCH -o fancy-%j.out
#SBATCH --gres=gpu:L40s:2
#SBATCH -t 0:20:00

echo `whoami` is running this script on `hostname`
```
would eneque the job with 4 CPU cores, with the job title `FancyJob`, store stderr and stdout into `fancy-id.err` and `fancy-id.out`, respectively, request two L40s GPUs, and set a time limit of 20 minutes. Possible output:

```
$ sbatch test.sbatch
Submitted batch job 2964
$ squeue -u $USER
             JOBID PARTITION     NAME     USER ST       TIME  NODES NODELIST(REASON)
              2964     short FancyJob   karppa  R       0:05      1 neptune
$ cat fancy-2964.out
karppa is running this script on neptune
```

### Using environments

#### Venv
In order to use venvs, one needs to activate them first. Since this is an ordinary shell script, all one needs to do is to issue `source env/bin/activate`. For example, suppose the environment `~/myenv/` contains NumPy. Furthermore, support we have the file `test.py` in our home directory with the following content:

```
import numpy as np

A = np.arange(1,17).reshape(4,4)
B = np.arange(17,33).reshape(4,4)
print(A @ B)
```

we can issue the following SBATCH script:

```
#!/bin/bash
#SBATCH -c 1
#SBATCH --mem-per-cpu=1G

source ~/myenv/bin/activate

python3 ~/test.py
```

So we simply activate the venv before executing the Python script, which should now have appropriate access to NumPy.

**Note.** The shebang `#!/bin/bash` is important! It tells Slurm to execute the script as a Bash script (in contrast to a regular Bourne shell script) which is needed for the `source` command to work.

#### Anaconda (Miniforge)

Similar to venvs, Anaconda environments need to be activated just like when executing interactively. So, we first issue `source ~/miniforge3/bin/activate`, followed by `conda activate myenv`.

Assume `test.py` is as in the previous section. We then issue the following SBATCH script:

```
#!/bin/bash
#SBATCH -c 1
#SBATCH --mem-per-cpu=1G

source ~/miniforge3/bin/activate
conda activate myenv

python3 ~/test.py
```
# Minerva cluster

![Minerva in the Heavens](minerva_in_the_heavens.jpg)

Андрей Иванович Иванов. 1820. *Минерва, парящая в эфире небес*.


## Introduction

Minerva is a compute cluster that is hosted by the CSE department at Chalmers /
GU.

## Software and Hardware

The cluster has the following nodes:

|Hostname  |CPU                                     |Memory |GPUs                   |Hard disk(s)                                  |
|----------|----------------------------------------|-------|-----------------------|----------------------------------------------|
|`io`      |INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|8×NVIDIA L4 (24 GiB)   |2×1.92 TB SSD (RAID-1)                        |
|`europa`  |INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|8×NVIDIA L4 (24 GiB)   |2×1.92 TB SSD (RAID-1)                        |
|`ganymede`|INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|8×NVIDIA L4 (24 GiB)   |2×1.92 TB SSD (RAID-1)                        |
|`callisto`|INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|8×NVIDIA L4 (24 GiB)   |2×1.92 TB SSD (RAID-1)                        |
|`uranus`  |INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|4×NVIDIA L40s (48 GiB) |2×1.92 TB SSD (RAID-1)                        |
|`neptune` |INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|512 GiB|4×NVIDIA L40s (48 GiB) |2×1.92 TB SSD (RAID-1)                        |
|`sensai`  |INTEL(R) XEON(R) GOLD 6548N (2×64 cores)|  1 TiB|4×NVIDIA H100L (94 GiB)|2×3.84 TB SSD (RAID-1)                        |
|`minerva` |Intel® Xeon® Silver 4410Y (1×12 cores)  |128 GiB|n/a                    |2×1.92 TB SSD (RAID-1) + 6×16 TiB HDD (RAIDZ2)|
|`jupiter` |INTEL(R) XEON(R) GOLD 6548N (2x64 cores)|512 GiB|n/a                    |2x1.92 TB SSD (RAID-1)                        |
|`saturn`  |INTEL(R) XEON(R) GOLD 6548N (2x64 cores)|512 GiB|n/a                    |2x1.92 TB SSD (RAID-1)                        |

`minerva` acts as the login node and the central point of access. The nodes `jupiter` and `saturn` are reserved for JupterHub use. The node `sensai` is reserved for research use. The nodes `io`, `europa`, `ganymede`, `callisto`, `uranus`, and `neptune` are accessible for batch processing through SLURM.

All nodes run Ubuntu Linux 24.04 LTS.

## Getting access

Access is available to students taking courses at CSE where compute is
needed, master thesis workers at CSE, and for CSE-affiliated staff.
To gain access, please [fill this form](https://forms.office.com/e/NLe5HDPGKY).
You will
need to provide:
- Your name
- Your email address
- Your CID
- Reason you need access (e.g., the name of the course you need access
  for)
  
If you have problems with the form or do not receive a confirmation
that you have been given access within a reasonable time frame, please send email to Matti Karppa <karppa(at)chalmers.se>. 
  
## Logging in

You can log in to the cluster by `ssh`'ing to `minerva.cse.chalmers.se`
using your CID:

```
ssh CID@minerva.cse.chalmers.se
```

Use your CID password.

The server is only available from Chalmers network. If you want to
access the cluster from outside Chalmers, you need to use Chalmers
VPN.

Once you can log in with your password, you can optionally [set up SSH
keys](https://www.cyberciti.biz/faq/how-to-set-up-ssh-keys-on-linux-unix/) and
add Minerva to your [SSH
config](https://linuxize.com/post/using-the-ssh-config-file/). If you use VSCode
you can use the [Remote - SSH](https://code.visualstudio.com/docs/remote/ssh)
extension to SSH to the cluster directly from within VSCode.

> :warning: **Note.** You must **not** run any compute-intensive jobs on the
> login node, all jobs shall be run through SLURM. Running intensive processes
> on the login node may lead to your access being revoked.

## JupyterHub

If you want to do interactive processing with Jupyter Notebooks, you can log in
directly to one of:
* [http://minerva.cse.chalmers.se/jupyter](http://minerva.cse.chalmers.se/jupyter)
* [http://minerva.cse.chalmers.se/saturn](http://minerva.cse.chalmers.se/saturn)

Use your CID and password to log in. As before, access is only available from
Chalmers network, and to access the server from outside networks requires the
use of VPN. Note that there is no GPU access for JupyterHub.

## Running a job

Minerva uses the [SLURM workload
manager](https://slurm.schedmd.com/overview.html) to schedule jobs on the
cluster nodes. To run a job on the cluster, you can execute a runfile with the
`sbatch` command:

```bash
sbatch my-script.sh
```

The runfile will contain both options for how to run it (lines that start with
`#SBATCH`) and commands to execute the programs. For example, here's a simple
runfile that runs a Python program on two CPUs:

```bash
#! /bin/bash

#SBATCH --cpus-per-task=2

python3 myjob.py
```

### Lenght of jobs
The default length of jobs is short to increase flow of jobs on the cluster, 
but also to avoid stuck jobs staying on the cluster for long. To run a longer
job you can run on the `long` queue. Do this by adding `-p long` when running
your job, i.e. `sbatch -p long my-script.sh`. The length is still limited to
1 day, if you need to run longer jobs you can save your results and load them
in a new job. It might be worth reading about the `--dependency` option in
this setting.

### Python environments

If you need packages that are not already available on the cluster, the easiest
way is to install them via [venv](https://docs.python.org/3/library/venv.html)
(a virtual environment).

To create a venv, go to your project directory and run `python3 -m venv .venv`.
This creates a `.venv/` hidden folder which will be where your Python packages
will be saved when they're installed from within the virtual environment.

To enter the virtual environment source the activation script:

```bash
source .venv/bin/activate
```

If this is successful, you should see `(.venv)` appear to the left of your
shell. Now, any packages installed with `pip` will be installed in the virtual
environment rather than in your home directory.

Sidenote: if you need to delete your virtual environment, all you need to do is
delete the `.venv/` folder.


To use your virtual environment within a `batch` job you can source the
activation script in your runfile before calling Python:

```
#! /bin/bash

#SBATCH --cpus-per-task=2

source /path/to/your/.venv/bin/activate
python3 myjob.py
```

Make sure to use the full path for the script.

### Using GPUs

If you need to use a GPU, you can add `#SBATCH --gres=gpu` to the runfile:

```
#! /bin/bash

#SBATCH --cpus-per-task=2
#SBATCH --gres=gpu

python3 myjob.py
```

You can request more than one GPU in the `--gres` option. For instance,
`#SBATCH --gres=gpu:3` will request 3 GPUs.

You can also specify the kind of GPU that you want. We have two types of GPUs in
the cluster:
- `L4` - [NVIDIA L4](https://www.nvidia.com/en-us/data-center/l4/) GPUs with
24GB of memory and ~7500 CUDA cores
- `L40s` - [NVIDIA L40](https://www.nvidia.com/en-us/data-center/l40/) GPUs with
48GB of memory and ~18000 CUDA cores

You can specify the GPU type in the `--gres` option as well. For example,
`#SBATCH --gres=gpu:L40s:2` will request 2 L40 GPUs.

> :warning: Note: there are a limited supply of GPU resources, so please don't
> request GPUs if you don't need them. There are lot more L4 GPUs (32 total)
> than L40 GPUs (8 total) so try to use L4 GPUs when possible.

> :warning: Note: PyTorch cannot make use of multiple GPUs automatically.
> You need to specifically modify your code to enable multi-GPU processing.
> If you do not know how to do this, only request one GPU.

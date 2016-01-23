using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyMachine {
  class Machine {
    private int[] memory = new int[32];
    private int program_counter = 0;
    private List<Instruction> program;

    public void LoadProgram(List<string> source) {
      program = new List<Instruction>();

      for(int i = 0; i < source.Count; i++) {
        string s = source[i];
        Instruction parsed = parse(s);
        if (parsed != null)
          program.Add(parsed);
        else
          Console.WriteLine("WARNING! Ignoring Malformed instruction (Line " + i + "): " + s);
      }
    }

    public int RunProgram() {
      program_counter = 0;

      while (program_counter < program.Count) {
        Instruction next = program[program_counter];
        program_counter++;
        next.Execute(this);
      }

      return memory[0];
    }

    private Instruction parse(string s) {
      if (s.Length <= 1)
        return null;

      switch (s[0]) {
        case 'Z':
          int n;
          if (Int32.TryParse(s.Substring(1), out n) && IsAddress(n))
            return new ZeroInstruction(n);
          break;
        case 'I':
          if (Int32.TryParse(s.Substring(1), out n) && IsAddress(n))
            return new IncrementInstruction(n);
          break;
        case 'J':
          int m;
          int t;
          string[] nums = s.Substring(1).Split(',');
          if (nums.Length == 3 &&
            Int32.TryParse(nums[0], out n) && IsAddress(n) &&
            Int32.TryParse(nums[1], out m) && IsAddress(m) &&
            Int32.TryParse(nums[2], out t) && t > 0)
            return new JumpEqualInstruction(n, m, t);
          break;
        default:
          break;
      }

      return null;
    }

    private bool IsAddress(int addr) {
      return addr >= 0 && addr <= memory.Length;
    }

    private abstract class Instruction {
      public abstract void Execute(Machine machine);
    }

    private class ZeroInstruction : Instruction {
      private int n;

      public ZeroInstruction(int n) {
        this.n = n;
      }

      public override void Execute(Machine machine) {
        machine.memory[n] = 0;
      }
    }

    private class IncrementInstruction : Instruction {
      private int n;

      public IncrementInstruction(int n) {
        this.n = n;
      }

      public override void Execute(Machine machine) {
        machine.memory[n]++;
      }
    }

    private class JumpEqualInstruction : Instruction {
      private int n;
      private int m;
      private int t;

      public JumpEqualInstruction(int n, int m, int t) {
        this.n = n;
        this.m = m;
        this.t = t;
      }

      public override void Execute(Machine machine) {
        if (machine.memory[n] == machine.memory[m]) {
          machine.program_counter = t;
        }
      }
    }
  }
}

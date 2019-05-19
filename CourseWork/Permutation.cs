﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    class Permutation
    {
        public void nextPermutation(int[] nums)
        {
            if (nums == null || nums.Length < 2)
                return;

            int p = 0;
            for (int i = nums.Length - 2; i >= 0; i--)
            {
                if (nums[i] < nums[i + 1])
                {
                    p = i;
                    break;
                }
            }

            int q = 0;
            for (int i = nums.Length - 1; i > p; i--)
            {
                if (nums[i] > nums[p])
                {
                    q = i;
                    break;
                }
            }

            if (p == 0 && q == 0)
            {
                reverse(nums, 0, nums.Length - 1);
                return;
            }

            int temp = nums[p];
            nums[p] = nums[q];
            nums[q] = temp;

            if (p < nums.Length - 1)
            {
                reverse(nums, p + 1, nums.Length - 1);
            }
        }

        public void reverse(int[] nums, int left, int right)
        {
            while (left < right)
            {
                int temp = nums[left];
                nums[left] = nums[right];
                nums[right] = temp;
                left++;
                right--;
            }
        }
    }
}
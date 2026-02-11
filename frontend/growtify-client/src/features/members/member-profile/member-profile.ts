import { Component, HostListener, inject, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EditableMember, Member } from '../../../types/member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe, FormsModule],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css',
})
export class MemberProfile implements OnInit, OnDestroy {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: BeforeUnloadEvent) {
    if (this.editForm?.dirty) {
      $event.preventDefault();
    }
  }
  protected memberService = inject(MemberService);
  private toast = inject(ToastService);
  protected editableMember: EditableMember = {
    displayName: '',
    description: '',
    city: '',
    country: ''
  };

  ngOnInit(): void {
    this.editableMember = {
      displayName: this.memberService.member()?.userName || '',
      description: this.memberService.member()?.description || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || ''
    }
  }

  updateProfile() {
    const currentMember = this.memberService.member();
    if (!currentMember) return;

    this.memberService.updateMember(this.editableMember).subscribe({
      next: () => {
        this.toast.success('Profile updated successfully');

        const updatedMember: Member = {
            ...currentMember,
            userName: this.editableMember.displayName,
            description: this.editableMember.description,
            city: this.editableMember.city,
            country: this.editableMember.country
        };

        this.memberService.editMode.set(false);
        this.memberService.member.set(updatedMember);

        this.editForm?.reset(this.editableMember);
      }
    });
  }

  ngOnDestroy(): void {
    if (this.memberService.editMode()) {
      this.memberService.editMode.set(false);
    }
  }
}

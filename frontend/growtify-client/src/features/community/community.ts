import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../core/services/member-service';
import { Member, MemberParams } from '../../types/member';
import { MemberCard } from "../members/member-card/member-card";
import { PaginatedResult } from '../../types/pagination';
import { Paginator } from "../../shared/paginator/paginator";
import { FilterModal } from '../members/filter-modal/filter-modal';

@Component({
  selector: 'app-community',
  imports: [MemberCard, Paginator, FilterModal],
  templateUrl: './community.html',
  styleUrl: './community.css',
})
export class Community implements OnInit {
  @ViewChild('filterModal') modal!: FilterModal;
  private memberService = inject(MemberService);
  protected paginatedMembers = signal<PaginatedResult<Member> | null>(null);
  protected memberParams = new MemberParams();

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.memberParams).subscribe({
      next: result => {
        this.paginatedMembers.set(result);
      }
    })
  }

  onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.memberParams.pageNumber = event.pageNumber;
    this.memberParams.pageSize = event.pageSize;
    this.loadMembers();
  }

  openModal() {
    this.modal.open();
  }

  onClose() {
    console.log('Modal closed');
  }

  onFilterChange(data: MemberParams) {
    this.memberParams = data;
    this.loadMembers();
  }

  resetFilters() {
    this.memberParams = new MemberParams();
    this.loadMembers();
  }
}

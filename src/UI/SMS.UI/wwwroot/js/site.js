/*initializing tooltip*/
(() => {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
})();


/* ------------------------------------------------------------
   TOASTR NOTIFICATIONS
   positionClass set to a custom "toast-center-screen" class
   (injected below) so success/error toasts appear in the exact
   middle of the screen, like a centered popup — toastr has no
   built-in true-center option (only top-center/bottom-center).
------------------------------------------------------------ */
(() => {
    const style = document.createElement('style');
    style.textContent = `
        #toast-container.toast-center-screen {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: auto;
        }
        #toast-container.toast-center-screen > div {
            margin: 0 auto 8px auto;
            float: none;
            width: 320px;
        }
    `;
    document.head.appendChild(style);
})();

toastr.options = {
    closeButton: true,
    debug: false,
    newestOnTop: true,
    progressBar: true,
    positionClass: "toast-center-screen", // shows toast in the middle of the screen
    preventDuplicates: true,
    tapToDismiss: true,
    showDuration: 300,
    hideDuration: 1000,
    timeOut: 4000,
    extendedTimeOut: 1000,
    showEasing: "swing",
    hideEasing: "linear",
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};

const alertNormal = {
    tClass: { error: "error", success: "success", info: "info", warning: "warning" },

    alert(title, message, type) {
        if (typeof toastr[type] !== "function") {
            console.warn(`alertNormal: unknown toast type "${type}"`);
            return;
        }
        toastr[type](message, title);
    },

    success(message, title = "Success") { this.alert(title, message, this.tClass.success); },
    error(message, title = "Error") { this.alert(title, message, this.tClass.error); },
    info(message, title = "Info") { this.alert(title, message, this.tClass.info); },
    warning(message, title = "Warning") { this.alert(title, message, this.tClass.warning); }
};
const an = alertNormal;

// Usage:
// an.success("Record saved successfully");
// an.error("Something went wrong");


/* ------------------------------------------------------------
   btnLdr — Button Loading State Helper
------------------------------------------------------------ */
const btnLdr = {
    ldrHTML: '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...',
    removeClass: '',
    addClass: '',

    Start(btn) {
        btn.attr('original-text', btn.html());
        btn.html(this.ldrHTML);
        btn.removeClass(this.removeClass).addClass(this.addClass);
        btn.prop('disabled', true);
    },

    Stop(btn) {
        btn.html(btn.attr('original-text'));
        btn.removeClass(this.addClass).addClass(this.removeClass);
        btn.prop('disabled', false);
    }
};


/* ------------------------------------------------------------
   modalPopup (mdlA) — Reusable Bootstrap Modal
------------------------------------------------------------ */
const modalPopup = {
    size: { small: "modal-sm", large: "modal-lg", xlarge: "modal-xl", xxlarge: "modal-xxl", full: "modal-fullscreen", default: "" },

    template(id, title) {
        return `
        <div class="modal fade" id="${id}" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5">${title}</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body"></div>
                </div>
            </div>
        </div>`;
    },

    show({ id = "myModal", title = "", content = "", size = this.size.default, parent = "body" } = {}) {
        const existing = document.getElementById(id);
        if (existing) {
            const inst = bootstrap.Modal.getInstance(existing);
            if (inst) inst.dispose();
            existing.remove();
        }

        $(parent).append(this.template(id, title));
        const modalEl = document.getElementById(id);
        $(modalEl).find(".modal-body").html(content);
        $(modalEl).find(".modal-dialog").addClass(size);

        const modal = new bootstrap.Modal(modalEl);
        modal.show();

        modalEl.addEventListener("hidden.bs.modal", () => {
            modal.dispose();
            modalEl.remove();
        }, { once: true });

        return modal;
    }
};
const mdlA = modalPopup;

// Usage:
// mdlA.show({ id: "masterDocType", title: "Document Master", content: htmlString, size: mdlA.size.large });


/* ------------------------------------------------------------
   AJAX BUTTON HANDLERS (Doc Type / KOT Orders modals)
------------------------------------------------------------ */
$('#btnDocType').on('click', function () {
    $.post("/GetAllDocs", function (data) {
        mdlA.show({ id: "masterDocType", title: "Document Master", content: data, size: mdlA.size.large });
    });
});

$('#btnKOTOrders').on('click', function () {
    $.post("/getKOTView", function (data) {
        mdlA.show({ id: "kotView", title: "KOT Orders", content: data, size: mdlA.size.xlarge });
    });
});


/* ------------------------------------------------------------
   MULTIPLE DASHBOARD ROLE SELECT
------------------------------------------------------------ */
var objDashboardListBase = [];
var objDashboardListBaseEdit = [];

$('#multipleDashboardRoleSelect').on('change', function () {
    var ids = $(this).val() || [];
    var roleDNames = $("#multipleDashboardRoleSelect option:selected").map((i, el) => el.textContent.trim()).get();
    var drawableDashboardRoleList = [];

    for (var i = 0; i < ids.length; i++) {
        var index = objDashboardListBase.findIndex(item => item.Id == ids[i]);

        if (index === -1) {
            var newRole = { Id: ids[i], RoleName: roleDNames[i] };
            drawableDashboardRoleList.push(newRole);
            objDashboardListBase.push(newRole);
        } else {
            var dRoleData = objDashboardListBase.find(m => m.Id == ids[i]);
            drawableDashboardRoleList.push({ Id: dRoleData.Id, RoleName: dRoleData.RoleName });
        }
    }
    drawDashboardRoleTable(drawableDashboardRoleList);
});

function drawDashboardRoleTable(drawableDashboardRoleList) {
    const $tbl = $('#tblDashboardRoleListCreate');
    $tbl.html('');

    if (drawableDashboardRoleList.length > 0) {
        drawableDashboardRoleList.forEach((role, m) => {
            const html = `<tr data-item-id='${role.Id}'>
                <input type='hidden' id='RolesList[${m}].Id' name='RolesList[${m}].Id' value='${role.Id}'/>
                <input type='hidden' id='RolesList[${m}].Name' name='RolesList[${m}].Name' value='${role.RoleName}'/>
                <td>${role.RoleName}</td></tr>`;
            $tbl.append(html);
        });
    } else {
        $tbl.append('<tr><td class="text-center" colspan="2">No Roles Selected</td></tr>');
    }
}


/* ------------------------------------------------------------
   setupPagination()
------------------------------------------------------------ */
function setupPagination(totalRecords, pageSize, currentPage, onPageChange) {
    const totalPages = pageSize === -1 ? 1 : Math.max(1, Math.ceil(totalRecords / pageSize));
    const $pagination = $("#pagination");
    $pagination.empty();

    const pageItem = (page, label, disabled = false, active = false) => `
        <li class="page-item ${disabled ? "disabled" : ""} ${active ? "active" : ""}">
            <a class="page-link" href="#" data-page="${page}">${label}</a>
        </li>`;

    const ellipsis = () => `<li class="page-item disabled"><span class="page-link">...</span></li>`;

    $pagination.append(pageItem(currentPage - 1, "Previous", currentPage === 1));

    if (currentPage > 3) {
        $pagination.append(pageItem(1, "1"));
        if (currentPage > 4) $pagination.append(ellipsis());
    }

    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);
    for (let i = startPage; i <= endPage; i++) {
        $pagination.append(pageItem(i, i, false, i === currentPage));
    }

    if (currentPage < totalPages - 2) {
        if (currentPage < totalPages - 3) $pagination.append(ellipsis());
        $pagination.append(pageItem(totalPages, totalPages));
    }

    $pagination.append(pageItem(currentPage + 1, "Next", currentPage === totalPages));

    $pagination.off("click", ".page-link").on("click", ".page-link", function (e) {
        e.preventDefault();
        const $li = $(this).closest(".page-item");
        if ($li.hasClass("disabled") || $li.hasClass("active")) return;

        const page = parseInt($(this).data("page"));
        if (!isNaN(page) && page >= 1 && page <= totalPages) {
            onPageChange(page);
        }
    });
}

// Usage:
// setupPagination(totalRecords, pageSize, currentPage, (page) => loadData(page));


/* ------------------------------------------------------------
   setTableSkeleton() / removeTableSkeleton()
------------------------------------------------------------ */
function setTableSkeleton(tableId, rowCount = 5, columnCount = 5) {
    const $tbody = $(`#${tableId} tbody`);
    if (!$tbody.length) {
        console.warn(`setTableSkeleton: no <tbody> found for #${tableId}`);
        return;
    }

    const rows = [];
    for (let i = 0; i < rowCount; i++) {
        const cells = Array.from({ length: columnCount }, () => `<td><div class="skeleton-line"></div></td>`).join("");
        rows.push(`<tr class="skeleton-row">${cells}</tr>`);
    }
    $tbody.html(rows.join(""));
}

function removeTableSkeleton(tableId) {
    $(`#${tableId} tbody tr.skeleton-row`).remove();
}

// Usage:
// setTableSkeleton("ordersTable", 8, 6);
// removeTableSkeleton("ordersTable");


/* ------------------------------------------------------------
   Box Model — openModal() / closeModal() (Alert / Confirmation)
------------------------------------------------------------ */
const bm = {
    tClass: {
        Error: "error",
        Warning: "warning",
        Success: "success",
        Info: "info",
        Confirmation: "confirmation"
    },

    images: {
        success: "/img/BoxModal/845646.png",
        warning: "/img/BoxModal/189667.png",
        error: "/img/BoxModal/564619.png",
        info: "/img/BoxModal/1828778.png",
        confirmation: "/img/BoxModal/190411.png"
    },

    alert(title, message, type, okText = null, action = null, cancelText = null) {
        const isConfirmation = type === bm.tClass.Confirmation;
        return openModal({
            type,
            title,
            message,
            image: bm.images[type] || "",
            actionText: okText || (isConfirmation ? "Yes, Proceed" : "OK"),
            cancelText: cancelText || "Cancel",
            action
        });
    }
};

function openModal(options) {
    return new Promise((resolve) => {
        document.getElementById("dynamicModal")?.remove();

        const modalHTML = `
        <div class="modal fade" id="dynamicModal" tabindex="-1" aria-labelledby="modalTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-body text-center p-4">
                        ${options.image ? `<img src="${options.image}" class="img-fluid mb-3" style="max-width:100px;">` : ""}
                        <h4 id="modalTitle">${options.title}</h4>
                        <p id="modalMessage">${options.message}</p>
                        <div id="modalButtons" class="mt-3"></div>
                    </div>
                </div>
            </div>
        </div>`;

        document.body.insertAdjacentHTML("beforeend", modalHTML);
        const modalElement = document.getElementById("dynamicModal");
        const buttonsContainer = document.getElementById("modalButtons");
        document.getElementById("main-content")?.setAttribute("inert", "");

        const finish = (result) => {
            options.action?.(result);
            resolve(result);
            closeModal();
        };

        if (options.type === bm.tClass.Confirmation) {
            buttonsContainer.innerHTML = `
                <button class="btn btn-success mx-2" id="modalOkButton">${options.actionText}</button>
                <button class="btn btn-secondary mx-2" id="modalCancelButton">${options.cancelText}</button>`;

            document.getElementById("modalOkButton").addEventListener("click", () => finish(true), { once: true });
            document.getElementById("modalCancelButton").addEventListener("click", () => finish(false), { once: true });
        } else {
            buttonsContainer.innerHTML = `<button class="btn btn-primary" id="modalCloseButton">${options.actionText}</button>`;
            document.getElementById("modalCloseButton").addEventListener("click", () => finish(true), { once: true });
        }

        const modal = new bootstrap.Modal(modalElement, { backdrop: "static", keyboard: false });
        modalElement.setAttribute("aria-hidden", "false");
        modal.show();

        modalElement.addEventListener("shown.bs.modal", () => {
            modalElement.querySelector("button, a, input")?.focus();
        }, { once: true });
    });
}

function closeModal() {
    const modalElement = document.getElementById("dynamicModal");
    if (!modalElement) return;

    const modalInstance = bootstrap.Modal.getInstance(modalElement);

    modalElement.addEventListener("hidden.bs.modal", () => {
        modalInstance?.dispose();
        document.getElementById("main-content")?.removeAttribute("inert");
        modalElement.remove();
    }, { once: true });

    if (modalInstance) {
        modalInstance.hide();
    } else {
        document.getElementById("main-content")?.removeAttribute("inert");
        modalElement.remove();
    }
}

// Usage:
// const confirmed = await bm.alert("Delete?", "This cannot be undone.", bm.tClass.Confirmation);
// bm.alert("Saved", "Record saved successfully.", bm.tClass.Success);


/* ------------------------------------------------------------
   formatDate()
------------------------------------------------------------ */
function formatDate(dateStr, {
    locale = "en-US",
    options = { year: "numeric", month: "short", day: "2-digit", hour: "2-digit", minute: "2-digit" },
    fallback = "N/A"
} = {}) {
    if (!dateStr) return fallback;

    const d = new Date(dateStr);
    if (isNaN(d.getTime())) {
        console.warn(`formatDate: could not parse date value:`, dateStr);
        return fallback;
    }

    return d.toLocaleString(locale, options);
}

// Usage:
// formatDate(order.createdOn)
// formatDate(order.createdOn, { options: { dateStyle: "medium" } })


/* ------------------------------------------------------------
   SortableTable()
------------------------------------------------------------ */
function SortableTable(tableSelector) {
    const $table = $(tableSelector);
    if (!$table.length) return;

    $table.find('th').each(function (index) {
        const $header = $(this);
        $header.css('cursor', 'pointer');
        $header.find('i.fa').remove();
        $header.append(` <i class="fa fa-sort" data-index="${index}" data-order="asc"></i>`);
    });

    const parseUniversalDate = (val) => {
        if (!val) return null;
        val = val.replace(',', '').trim();

        let d = new Date(val);
        if (!isNaN(d)) return d;

        const parts = val.split(' ');
        if (parts.length >= 3) {
            const months = {
                Jan: 0, Feb: 1, Mar: 2, Apr: 3, May: 4, Jun: 5,
                Jul: 6, Aug: 7, Sep: 8, Oct: 9, Nov: 10, Dec: 11
            };

            if (months[parts[1]] !== undefined) {
                let day = parseInt(parts[0]);
                let month = months[parts[1]];
                let year = parseInt(parts[2]);
                let hour = 0, min = 0;

                if (parts[3]) {
                    let [h, m] = parts[3].split(':');
                    hour = parseInt(h);
                    min = parseInt(m);

                    let period = parts[4];
                    if (period === 'PM' && hour !== 12) hour += 12;
                    if (period === 'AM' && hour === 12) hour = 0;
                }

                return new Date(year, month, day, hour, min);
            }
        }
        return null;
    };

    const extractMixedNumber = (str) => {
        const match = str.match(/\d+/g);
        return match ? parseInt(match.join(''), 10) : null;
    };

    const getCellValue = (row, columnIndex) => {
        const $td = $(row).children('td').eq(columnIndex);
        if (!$td.length) return '';

        const rawDate = $td.data('date');
        if (rawDate) {
            const d = new Date(rawDate);
            if (!isNaN(d)) return d.getTime();
        }

        let text = $td.text().replace(/\u00a0/g, ' ').replace(/,/g, '').trim();
        if (!text) return '';

        const num = parseFloat(text);
        if (!isNaN(num) && text.match(/^[-+]?\d*\.?\d+$/)) return num;

        const parsedDate = parseUniversalDate(text);
        if (parsedDate) return parsedDate.getTime();

        const mixedNum = extractMixedNumber(text);
        if (mixedNum !== null) return mixedNum;

        return text.toLowerCase();
    };

    $table.off('click', 'th').on('click', 'th', function () {
        const $icon = $(this).find('i.fa');
        const columnIndex = parseInt($icon.data('index'));
        const currentOrder = $icon.data('order') || 'asc';
        const newOrder = currentOrder === 'asc' ? 'desc' : 'asc';

        $table.find('i.fa').removeClass('fa-sort-up fa-sort-down').addClass('fa-sort').data('order', 'asc');
        $icon.removeClass('fa-sort').addClass(newOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down').data('order', newOrder);

        const rowsArray = $table.find('tbody tr').toArray();

        rowsArray.sort((a, b) => {
            let valA = getCellValue(a, columnIndex);
            let valB = getCellValue(b, columnIndex);

            if (valA === '') return 1;
            if (valB === '') return -1;

            let result = 0;
            if (typeof valA === "number" && typeof valB === "number") {
                result = valA - valB;
            } else {
                result = valA.toString().localeCompare(valB.toString(), undefined, { numeric: true, sensitivity: 'base' });
            }
            return newOrder === 'asc' ? result : -result;
        });

        $.each(rowsArray, function (_, row) {
            $table.children('tbody').append(row);
        });
    });
}


/* ------------------------------------------------------------
   convertAmountToWords()
------------------------------------------------------------ */
function convertAmountToWords(amount) {
    if (!amount || isNaN(amount) || amount <= 0) return "";
    if (amount > 999999999999) return "Amount is too large";

    const belowTwenty = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
        "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"];
    const tens = ["", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];
    const scales = ["", "Thousand", "Lakh", "Crore"];

    function convert(n) {
        if (n < 20) return belowTwenty[n];
        if (n < 100) return tens[Math.floor(n / 10)] + (n % 10 ? " " + belowTwenty[n % 10] : "");
        return belowTwenty[Math.floor(n / 100)] + " Hundred" + (n % 100 ? " " + convert(n % 100) : "");
    }

    let words = "";
    let num = Math.floor(amount);
    let scaleIndex = 0;

    while (num > 0) {
        let temp;
        if (scaleIndex === 0) {
            temp = num % 1000;
            num = Math.floor(num / 1000);
        } else {
            temp = num % 100;
            num = Math.floor(num / 100);
        }

        if (temp > 0) {
            words = convert(temp) + " " + scales[scaleIndex] + " " + words;
        }
        scaleIndex++;
    }

    return words.trim() + " Rupees Only";
}

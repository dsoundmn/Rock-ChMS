﻿(function ($) {
    'use strict';
    window.Rock = window.Rock || {};
    Rock.controls = Rock.controls || {};

    Rock.controls.itemPicker = (function () {
        var ItemPicker = function (options) {
                this.options = options;
            },
            exports;

        ItemPicker.prototype = {
            constructor: ItemPicker,
            initialize: function () {
                var $control = $('#' + this.options.controlId),
                    $tree = $control.find('.treeview'),
                    treeOptions = {
                        multiselect: this.options.allowMultiSelect,
                        restUrl: this.options.restUrl,
                        restParams: this.options.restParams,
                        expandedIds: this.options.expandedIds
                    },
                    $hfItemIds = $('#hfItemId_' + this.options.controlId),
                    $hfExpandedIds = $('#hfInitialItemParentIds_' + this.options.controlId);

                if (typeof this.options.mapItems === 'function') {
                    treeOptions.mapping = {
                        mapData: this.options.mapItems
                    };
                }

                $control.find('.scroll-container').tinyscrollbar({ size: 120 });
                // Since some hanlers are "live" events, they need to be bound before tree is initialized
                this.initializeEventHandlers();

                if ($hfItemIds.val() && $hfItemIds !== '0') {
                    treeOptions.selectedIds = $hfItemIds.val().split(',');
                }
                
                if ($hfExpandedIds.val()) {
                    treeOptions.expandedIds = $hfExpandedIds.val().split(',');
                }

                $tree.rockTree(treeOptions);
                this.updateScrollbar();
            },
            initializeEventHandlers: function () {
                var self = this,
                    $control = $('#' + this.options.controlId),
                    $spanNames = $control.find('.selected-names'),
                    $hfItemIds = $('#hfItemId_' + this.options.controlId),
                    $hfItemNames = $('#hfItemName_' + this.options.controlId);

                // Bind tree events
                $control.find('.treeview')
                    .on('rockTree:selected', function () {
                        var rockTree = $(this).data('rockTree'),
                            selectedNodes = rockTree.selectedNodes,
                            selectedIds = [],
                            selectedNames = [];

                        $.each(selectedNodes, function (index, node) {
                            selectedIds.push(node.id);
                            selectedNames.push(node.name);
                        });

                        $hfItemIds.val(selectedIds.join(','));
                        $hfItemNames.val(selectedNames.join(','));
                        $spanNames.text(selectedNames.join(', '));
                    })
                    .on('rockTree:expand rockTree:collapse', function () {
                        self.updateScrollbar();
                    });

                $control.find('a.picker-label').click(function (e) {
                    e.preventDefault();
                    $control.find('.picker-menu').first().toggle();
                    self.updateScrollbar();
                });

                $control.hover(
                    function () {
                        if ($hfItemIds.val() && $hfItemIds.val() !== '0') {
                            $control.find('.picker-select-none').show();
                        }
                    },
                    function () {
                        $control.find('.rock-picker-select-none').fadeOut(500);
                    });

                $control.find('.picker-cancel, .picker-btn').click(function () {
                    $(this).closest('.picker-menu').slideUp();
                });

                $control.find('.picker-select-none').click(function (e) {
                    e.stopImmediatePropagation();
                    var rockTree = $control.find('.treeview').data('rockTree');
                    rockTree.clear();
                    $hfItemIds.val('');
                    $hfItemNames.val('');
                    $spanNames.text(self.options.defaultText);
                    return false;
                });
            },
            updateScrollbar: function () {
                var $container = $('#' + this.options.controlId).find('.scroll-container'),
                    $dialog = $('#modal-scroll-container'),
                    dialogTop,
                    pickerTop,
                    amount;

                if ($container.is(':visible')) {
                    $container.tinyscrollbar_update('relative');

                    if ($dialog.length > 0 && $dialog.is(':visible')) {
                        dialogTop = $dialog.offset().top;
                        pickerTop = $container.offset().top;
                        amount = pickerTop - dialogTop;

                        if (amount > 160) {
                            $dialog.tinyscrollbar_update('bottom');
                        }
                    }
                }
            }
        };

        exports = {
            defaults: {
                id: 0,
                controlId: null,
                restUrl: null,
                restParams: null,
                allowMultiSelect: false,
                defaultText: '<none>',
                selectedIds: null,
                expandedIds: null
            },
            controls: {},
            initialize: function (options) {
                var settings,
                    itemPicker;

                if (!options.controlId) throw 'controlId must be set';
                if (!options.restUrl) throw 'restUrl must be set';

                settings = $.extend({}, exports.defaults, options);

                if (!settings.defaultText) {
                    settings.defaultText = exports.defaults.defaultText;
                }

                itemPicker = new ItemPicker(settings);
                exports.controls[settings.controlId] = itemPicker;
                itemPicker.initialize();
            }
        };

        return exports;
    }());
}(jQuery));